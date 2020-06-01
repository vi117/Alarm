using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;
using Terminal.Gui;
using ViewModel;

namespace ConsoleView
{
    class Program
    {
        static ViewModel.ViewModel viewModel;

        class TreeViewRender : View
        {
            int seletedY;
            int total;
            public TreeViewRender(int x, int y, int w, int h) : base(new Rect(x, y, w, h))
            {
                CanFocus = true;
                seletedY = -1;
                total = viewModel.TreeView.Count();
                viewModel.PropertyChanged += (o, e) => { SetNeedsDisplay(); };
                foreach(var c in viewModel.TreeView)
                {
                    c.PropertyChanged += (o, e) => SetNeedsDisplay();
                }
            }
            public override void PositionCursor()
            {
                if (seletedY < 0)
                {
                    seletedY = 0;
                    Move(0, seletedY);
                    SetNeedsDisplay();
                }
                Move(0, seletedY);
            }
            public override void Redraw(Rect region)
            {
                Driver.SetAttribute(ColorScheme.Normal);
                Move(region.Top, region.Left);
                int i = 0;
                foreach (var c in viewModel.TreeView)
                {
                    DrawOne(region, i, c.Title);
                    if (c.IsExpanded)
                    {
                        i++;
                        foreach (var f in c.SiteModels)
                        {
                            DrawOne(region, i, f.Title, true);
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            private void DrawOne(Rect region,int i,string name,bool f = false) {
                if (i == seletedY) Driver.SetAttribute(ColorScheme.Focus);
                Move(region.Left, region.Top + i);
                Driver.AddStr(name);
                if (i == seletedY) Driver.SetAttribute(ColorScheme.Normal);
            }
            public override bool ProcessHotKey(KeyEvent keyEvent)
            {
                if (keyEvent.Key == Key.CursorUp && HasFocus)
                {
                    if (seletedY == 0) return false;
                    CursorUp();
                    return true;
                }
                else if(keyEvent.Key == Key.CursorDown && HasFocus)
                {
                    if (seletedY >= total-1) return false;
                    CursorDown();
                    return true;
                }
                else if(keyEvent.Key == Key.CursorLeft && HasFocus)
                {
                    CursorLeft();
                    return true;
                }
                else if (keyEvent.Key == Key.CursorRight && HasFocus)
                {
                    CursorRight();
                    return true;
                }
                return base.ProcessHotKey(keyEvent);
            }
            private void CursorUp()
            {
                seletedY -= 1;
                Move(0, seletedY);
                SetNeedsDisplay();
            }
            private void CursorDown()
            {
                seletedY += 1;
                Move(0, seletedY);
                SetNeedsDisplay();
            }
            private void CursorLeft()
            {
                var item = SelectedItem;
                if(item is CategoryViewModel categoryView)
                {
                    if (categoryView.IsExpanded)
                    {
                        categoryView.IsExpanded = false;
                        total -= categoryView.SiteModels.Count();
                        //SetNeedsDisplay();
                    }
                }
            }
            private void CursorRight()
            {
                var item = SelectedItem;
                if (item is CategoryViewModel categoryView)
                {
                    if (!categoryView.IsExpanded)
                    {
                        categoryView.IsExpanded = true;
                        total += categoryView.SiteModels.Count();
                        //SetNeedsDisplay();
                    }
                }
            }
            private object SelectedItem {
                get
                {
                    if (seletedY < 0) return null;
                    int i = 0;
                    foreach(var c in viewModel.TreeView)
                    {
                        if (i == seletedY) return c;
                        if (c.IsExpanded)
                        {
                            if (c.SiteModels.Count() + i < seletedY)
                            {
                                i += c.SiteModels.Count();
                            }
                            else
                            {
                                foreach(var f in c.SiteModels)
                                {
                                    i++;
                                    if(i == seletedY)
                                        return f;
                                }
                            }
                        }
                        i++;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }

        static void Main(string[] args)
        {
            PlatformSevice.Instance = new DefaultPlatformService();
            viewModel = /*new MockViewModel();*/ViewModel.DB.ViewModelLoader.LoadViewModel();

            Application.Init();
            var top = Application.Top;
            var menu = new MenuBar(
                new MenuBarItem[]{
                    new MenuBarItem(
                       "File",
                        new MenuItem[]{
                            new MenuItem("Quit","",()=>{top.Running = false; })
                        }
                    )
                }
            );
            top.Add(menu);
            var win = new Window("Alarm") {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            {
                int pos = 1;
                Func<string, int> addButton = (name) =>
                {
                    win.Add(new Button(pos, 0, name));
                    pos += name.Length + 4;
                    return 0;
                };
                addButton("Add");
                addButton("Reload");
                addButton("Translate");
                addButton("Delete");
                addButton("Edit");
                addButton("Setting");
            }
            win.Add(new TreeViewRender(1,1,50,50));
            top.Add(win);
            Application.Run();
            /*Model.DB.AppDBContext.Test();
            RSSFetcher obj = new RSSFetcher(@"https://media.daum.net/syndication/economic.rss");
            
            var q = new Queue<Document>();
            DocumentPublisher publisher = new DocumentPublisher();
            publisher.AddFetcher(new RSSFetcher(@"https://media.daum.net/syndication/economic.rss"));
            publisher.OnPublished += (o, e) => {    
                foreach(var doc in e.Documents)
                {
                    q.Enqueue(doc);
                }
            };
            while (true)
            {
                while (true)
                {
                    if (q.Count > 0)
                    {
                        var doc = q.Dequeue();
                        Console.WriteLine(doc.Title);
                    }
                    else break;
                }
                Thread.Sleep(100);
            }*/
        }
    }
}
