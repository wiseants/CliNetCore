using CommandLine;
using CliNetCore.Interfaces;

namespace CliNetCore.Cores.Commands
{
    /// <summary>
    /// 종료 명령.
    /// </summary>
    [Verb("exit", HelpText = "Exit program.")]
    public class ExitCommand : IAction
    {
        #region Properties

        /// <summary>
        /// 유효성.
        /// </summary>
        public bool IsValid => true;

        #endregion

        #region Public methods

        /// <summary>
        /// -1 반환은 프로그램 종료.
        /// </summary>
        /// <returns></returns>
        public int Action()
        {
            return -1;
        }

        #endregion
    }
}
