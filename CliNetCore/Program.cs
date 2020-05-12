using CommandLine;
using CliNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CliNetCore
{
    public class Program
    {
        public static readonly char[] DELIMITER_CHARS = { ' ' };

        public static void Main(string[] args)
        {
            // IAction 구현 클래스를 전부 사용.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IAction).IsAssignableFrom(p));

            // 파라미터가 있는 경우 1회 실행 후 종료하는 모드.
            // 파라미터가 없는 경우 지속적인 명령어 입력이 가능한 모드. 종료 명령으로 종료.
            bool isContinuous = args.Length <= 0;

            do
            {
                IEnumerable<string> currentArgs = args.OfType<string>().ToList();
                if (currentArgs.Count() <= 0)
                {
                    // 새로운 파라미터 입력.
                    Console.Write("<");
                    currentArgs = Console.ReadLine().Split(DELIMITER_CHARS);
                }

                int commandResult = Parser.Default.ParseArguments(currentArgs, types.ToArray())
                    .MapResult((IAction opts) =>
                    {
                        if (opts.IsValid == false)
                        {
                            Console.WriteLine("Invalid command.");
                            return 0;
                        }

                        return opts.Action();
                    }, errs => 0);

                isContinuous &= commandResult >= 0;
                if (isContinuous == true)
                {
                    // 지속적으로 사용하는 경우 결과값 출력.
                    Console.WriteLine(">{0}", commandResult);
                }
            } while (isContinuous == true);
        }
    }
}
