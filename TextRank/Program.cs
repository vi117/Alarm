﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextRankTest
{
    class Program
    {
        static string test = @"타입스트립트(TypeScript)는 자바스크립트(JavaScript)의 인기를 기반으로 하지만 기업 개발자를 좀 더 행복하고 생산적으로 만들기 위해 기능을 추가한 객체 지향 프로그래밍 언어다.

타입스크립트는 인기 있는 자바스크립트 프로그래밍 언어의 변형으로, 기업 개발자에게 중요한 몇 가지 중요한 기능을 추가한다. 특히 타입스크립트는 강력한 형식(strongly typed)의 언어다. 즉, 변수와 기타 데이터 구조가 문자열이나 블린(boolean)과 같은 특정 형식을 갖도록 프로그래머가 선언할 수 있으며, 타입스크립트가 값의 유효성을 확인한다. 느슨한 형식(loosely typed)의 자바스크립트에서는 불가능한 부분이다.

타입스크립트의 강력한 형식 지정은 특히 방대한 엔터프라이즈 스케일의 코드베이스를 다룰 때 개발자의 효율성을 높여주는 여러 기능의 기반이 된다. 인터프리트 언어인 자바스크립트와 달리 타입스크립트는 컴파일되므로 실행 전에 오류가 포착된다. 백그라운드 증분 컴파일을 실행하는 IDE가 코딩 프로세스 중에 이런 오류를 잡아낼 수 있다.

자바스크립트와 이와 같은 중대한 차이점에도 불구하고 자바스크립트를 실행할 수 있는 모든 곳에서 타입스크립트를 실행할 수 있다. 타입스크립트가 바이너리 실행 파일이 아닌 표준 자바스크립트로 컴파일되기 때문이다. 지금부터 타입스크립트에 대해 세부적으로 살펴보자.

타입스크립트는 자바스크립트의 확대집합이다. 모든 올바른 자바스크립트 코드는 올바른 타입스크립트 코드이기도 하지만 타입스크립트에는 자바스크립트에 없는 언어 기능도 있다. 타입스크립트에만 있는 가장 대표적인 기능은 앞서 언급했듯이 강력한 형식이다. 이는 타입스크립트라는 이름의 유래이기도 하다. 타입스크립트 변수는 문자열, 수, 불린과 같은 형식과 연결된다. 형식은 컴파일러에 변수가 어떤 종류의 데이터를 담을 수 있는지를 알려준다. 또한 타입스크립트는 형식 추론을 지원하며 잡다한 형식을 모두 담는 any 형식도 포함하므로 프로그래머가 명시적으로 변수에 형식을 할당할 필요가 없다.

타입스크립트는 객체 지향 프로그래밍에 맞게 설계된 반면 자바스크립트는 그렇지 않다. 상속(Inheritance), 액세스 제어 등 자바스크립트에서 직관적이지 않은 개념을 타입스크립트에서는 간단히 구현할 수 있다. 또한 타입스크립트에서는 인터페이스를 구현할 수 있는데, 자바스크립트에서 이는 대체로 무의미한 개념이다.

물론 타입스크립트로 코딩할 수 있는 기능이라면 자바스크립트로도 코딩할 수 있다. 타입스크립트의 컴파일 방식은 일반적인 방식, 예를 들어 C++가 지정된 하드웨어에서 실행 가능한 바이너리 실행 파일로 컴파일되는 것과는 다르기 때문이다. 타입스크립트 컴파일러는 타입스크립트 코드를 기능적으로 동일한 자바스크립트로 크랜스코딩한다.

깃커넥티드(GitConnected)의 숀 맥스웰이 쓴 문서에는 객체 지향 타입스크립트 코드 스니펫과 이에 대응하는 자바스크립트 예제가 잘 나와 있다. 결과적으로 얻게 되는 자바스크립트는 웹 브라우저에서 Node.js가 설치된 서버에 이르기까지 자바스크립트 코드를 실행할 수 있는 어디에서나 실행 가능하다.

타입스크립트가 좋다 해도 그 결과물이 자바스크립트라면, 굳이 타입스크립트를 사용하는 이유는 무엇일까? 이 질문에 답하기 위해서는 타입스크립트의 기원과 용도를 살펴봐야 한다.

타입스크립트는 마이크로소프트가 내부적으로 개발하다가 2012년 오픈소스로 출시됐다. 마이크로소프트는 지금도 타입스크립트 프로젝트의 책임자이자 주 개발사다. 이와 관련해 “타입스크립트가 만들어진 중대한 동기 중 하나는 자바스크립트를 사용해 마이크로소프트 제품을 개발하고 유지하고자 노력했던 마이크로소프트 내 다른 팀의 경험”이라는 흥미로운 기사가 있었다.

당시 마이크로소프트는 빙 지도(Bing Maps)를 구글 지도(Google Maps)의 경쟁 제품으로 키우는 일과 오피스 제품군의 웹 버전을 출시하기 위한 일에 매달리고 있었는데, 두 가지 과제에 사용된 주 개발 언어가 바로 자바스크립트였다.

그러나 개발자들은 마이크로소프트 플래그십 제품 수준의 규모에서는 자바스크립트로 앱을 만들기가 어렵다는 사실을 알게 됐다. 그래서 자바스크립트 환경에서 실행되는 엔터프라이즈급 애플리케이션을 더 쉽게 구축하기 위한 목적으로 타입스크립트를 개발했다. 공식 타입스크립트 프로젝트 사이트에 걸린 “확장되는 자바스크립트(JavaScript that scales)”라는 표어에 담긴 의미가 바로 이것이다.

이런 종류의 작업에 기본 자바스크립트보다 타입스크립트가 더 나은 이유가 무엇일까? 객체 지향 프로그래밍의 장점에 대한 언쟁을 하자면 끝도 없지만, 현실적인 면을 보면 엔터프라이즈 프로젝트에 참여해 작업하는 많은 소프트웨어 개발자가 익숙하고, 프로젝트의 규모가 커지면서 코드 재사용에도 유리하다는 점이다.

또한 개발자의 생산성을 높여주는 다양한 툴도 무시할 수 없다. 앞서 언급했듯이 대부분의 엔터프라이즈 IDE는 작업하는 과정에서 오류를 찾을 수 있는 백그라운드 증분 컴파일을 지원한다. 코드의 구문이 올바르다면 오류가 있어도 트랜스파일이 되지만 결과로 얻게 되는 자바스크립트는 제대로 작동하지 않을 수 있다. 어떤 면에서는 맞춤법 검사와 비슷한 오류 확인이라고 생각하면 된다.

또한 이와 같은 IDE는 프로젝트에 깊이 들어가면서 코드를 리팩토링하는 데도 도움이 된다.

간단히 말해 타입스크립트는 자바와 같은 언어의 엔터프라이즈 기능과 툴이 필요하지만 자바스크립트 환경에서 코드를 실행해야 하는 경우에 사용된다. 이론적으로 타입스크립트 컴파일러를 통해 생성되는 표준 자바스크립트를 개발자가 직접 쓸 수도 있다. 그러나 시간이 훨씬 더 오래 걸리고, 그렇게 나온 코드베이스는 대규모 팀에서 모두가 이해하고 디버그하기도 더 어려울 것이다.

타입스크립트에는 또 다른 비장의 무기도 있다. 특정 자바스크립트 런타임 환경, 브라우저 또는 언어 버전을 타겟팅하도록 컴파일러를 설정할 수 있다는 것이다. 정상적으로 작성된 모든 자바스크립트 코드는 타입스크립트 코드이기도 하므로, 예를 들어 여러가지 새로운 구문 특성이 포함된 ECMAScript 2015 사양에 따라 작성된 코드를 가지고 와서 자바스크립트 레거시 버전과 호환되는 자바스크립트 코드로 컴파일할 수 있다.

타입스크립트를 사용해 볼 준비가 되었는가? 설치는 쉽다. 개발 컴퓨터에서 이미 Node.js를 사용 중이라면 Node.js 패키지 관리자인 NPM을 사용해 설치할 수 있다. 공식 타입스크립트 5분 요약 가이드에 설치 과정이 나온다.

원하는 IDE에 플러그인으로 타입스크립트를 설치할 수도 있다. 이렇게 하면 앞서 언급한 툴의 이점을 얻게 되고, 타입스크립트를 자바스크립트로 컴파일하는 프로세스도 알아서 처리된다. 타입스크립트는 마이크로소프트에서 개발한 언어인 만큼 비주얼 스튜디오와 비주얼 스튜디오 코드용으로 고품질의 플러그인이 준비돼 있다.

그러나 타입스크립트는 오픈소스 프로젝트이므로 이클립스(Eclipse)와 같은 오픈소스 IDE부터 유명한 텍스트 편집기인 빔(Vim)에 이르기까지 온갖 곳에 들어가 있기도 하다. 또한 깃허브에서 전체 프로젝트를 살펴보고 다운로드할 수 있다.
 

타입스크립트를 설치하고 나면 이제 타입스크립트 구문의 기본을 파악할 차례다. 타입스크립트의 기반은 자바스크립트이므로 시작하기 전에 자바스크립트에 익숙해질 필요가 있다. 주 관심사는 당연히 타입스크립트를 고유한 언어로 만들어주는 타입스크립트만의 기능일 것이므로 그 부분을 살펴보자.

타입스크립트에서 가장 중요한 구문 특성은 물론 형식 시스템이다. 타입스크립트는 다음과 같은 여러가지 기본 형식을 지원한다.
 
변수에 명시적으로 형식을 할당하는 방법은 두 가지다. 첫 번째는 꺾쇠괄호 구문이다.

두 번째는 as 구문이다.


타입스크립트 문서에서 발췌한 2개의 코드 스니펫은 기능적으로 동일하다. 2개 모두 를 any 형식의 변수로 선언하고 “”을 값으로 할당한 다음 를 수로 선언하고  내용물의 길이를  값으로 할당한다.    

타입스크립트 형식은 추론에 의한 설정도 가능하다. 즉, x의 값을 7로 설정하면서 x의 형식을 설정하지 않으면 컴파일러는 x가 수라고 전제한다. 일부 상황에서 컴파일러가 any 유형을 추론할 수도 있지만 컴파일 플래그를 사용해 이런 추론을 차단할 수 있다.

타입스크립트 형식 시스템은 그 내용이 상당히 방대해 여기서 다룰 수 있는 범위를 넘어선다. 여러가지 고급 형식과 유틸리티 형식이 있으며 이 가운데에는 변수의 형식이 여러가지 지정된 형식 중 하나가 되도록 하는 유니온 형식(union types), 기존 형식을 기반으로 생성할 수 있고 기존 형식의 각 속성을 동일한 방식으로 일률적으로 변형하는 매핑된 형식(mapped type)이 있다.

예를 들어 숫자 또는 불린 중 하나가 되도록 하고 문자열이나 다른 형식은 되지 않도록 하려는 변수에 대해 유니온 형식을 만들거나, 배열의 모든 요소를 읽기 전용으로 설정하는 매핑된 형식을 만들 수 있다.

대부분의 객체 지향 언어가 그렇듯이 타입스크립트에도 사용자가 자기만의 형식을 정의할 수 있게 해주는 인터페이스가 있다. 인터페이스는 객체가 갖는 속성과 함께 이와 같은 속성에 연결되는 형식을 설정한다. 타입스크립트 인터페이스는 두 가지 선택적 속성을 가질 수 있다. 구문에 대한 자세한 내용은 를 참고하면 된다.


타입스크립트 역시 자바, C#과 같은 객체 지향 언어와 마찬가지로 제네릭(generics) 개념이 있다(C++에서 제네릭에 해당하는 기능은 템플릿이다). 타입스크립트에서 제네릭 구성요소는 하나의 형식이 아니라 해당 구성요소가 호출된 코드 위치에 따라 다양한 형식으로 작동할 수 있다. 타입스크립트 문서에 아주 간단한 가 나와 있다. 먼저 인수를 취한 다음 즉시 반환하는 이 함수를 보자.
함수는  형식으로 정의되었으므로 아무 형식의 인수나 받는다. 함수가 반환하는 인수의 형식은다. 제네릭을 사용하는 함수 버전은 다음과 같다.
이 코드에는 형식 변수인 T가 포함된다. 이 변수는 들어오는 인수의 형식을 캡처해서 나중에 사용할 수 있도록 저장한다.
제네렉에는 이 외에도 많은 특성이 있으며 이런 특성은 대규모 엔터프라이즈 프로젝트에서 코드 재사용을 위한 중요한 요소이기도 하다. 자세한 내용은 타입스크립트 문서에서 볼 수 있다.
객체 지향 프로그래밍에서 클래스(classes)는 기능을 상속하며 객체의 구성 요소 역할을 한다. 자바스크립트는 원래 클래스를 사용하지 않고, 대신 함수와 프로토타입 기반의 상속에 의존했지만 ECMAScript 2015 표준 버전에서 클래스 개념이 추가됐다.
타입스크립트는 이전부터 클래스를 포함했으며 현재 자바스크립트와 동일한 구문을 사용한다. 타입스크립트 컴파일러의 혜택 중 하나는 자바스크립트 클래스가 있는 코드를 2015 이전의 표준에 부합하는 레거시 자바스크립트 코드로 변환할 수 있다는 것이다.
타입스크립트에서 날짜와 시간을 구하고 설정하는 데 사용할 수 있는 메서드와 객체는 여러 가지이며 대부분은 자바스크립트에서 물려받은 것이다. JavaTPoint에서 작동 방식에 대한 자세한 설명을 볼 수 있다.
더 깊은 내용을 원한다면? 다음 타입스크립트 자습서를 활용해보자.";
        static void Main(string[] args)
        {
            var textRanker = new SummaryDocument.TextRank();

            string[] tests = test.Split(new char[] { '\n', '.', '?' })
                .Select((s) => s.Trim())
                .Where((s) => s.Length != 0)
                .ToArray();
            textRanker.MakeTokens(tests);

            var ret = textRanker.GetSimilarityMatrix();

            //O(k)+O(t)+O(k*k*mt*mt)
            double max = 0;
            int mj = 0, mi = 0;
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                for (int j = 0; j < ret.GetLength(1); j++)
                {
                    if (i != j && max < ret[i, j])
                    {
                        max = ret[i, j];
                        mi = i;
                        mj = j;
                    }
                }
            }
            Console.WriteLine(tests[mi]);
            Console.WriteLine(tests[mj]);

            Console.ReadLine();
        }
        private static void Benchmark(Action act, int iterations)
        {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                act.Invoke();
            }
            sw.Stop();
            Console.WriteLine((sw.ElapsedMilliseconds / iterations).ToString());
        }
    }
}
