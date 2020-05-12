# 간단한 CLI(Command Line Interface) 프로그램 예제.

간단한 명령으로 테스트를 하거나 인터페이스로 이용하기 위해 만든 프로젝트.

# 빌드

Visual studio 2017에서 로드해서 그냥 빌드.
다른 버전에서 안해봄.

# 사용 라이브러리

CommandLineParser 커맨드라인 파라미터를 입력받기 위한 오픈 소스 사용.
https://github.com/commandlineparser/commandline

# 명령 추가 방법

아래 예제와 같이 명령어 클래스를 구현.
주석문 참고.

```cs
namespace CliNetCore.Models.Commands
{
    /// <summary>
    /// 명령어 예제.
    /// </summary>
    [Verb("example", HelpText = "Command example.")]
    public class ExampleCommand : IAction
    {
        /// <summary>
        /// 필수 옵션.
        /// </summary>
        [Option('r', "required", Required = true, HelpText = "is a required parameter.")]
        public string Ip
        {
            get;
            set;
        }

        /// <summary>
        /// 선택 옵션.
        /// </summary>
        [Option('o', "option", Required = false, HelpText = "is a optional parameter.")]
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// 명령 실행 메소드.
        /// </summary>
        /// <returns>-1:종료, 그 외는 처리 결과.</returns>
        public int Action()
        {
            Console.WriteLine("Executed example command. Required:{0}, Option:{1}", Ip, Port);
            return 1;
        }
    }
}
```