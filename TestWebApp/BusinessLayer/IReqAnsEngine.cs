using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.BusinessLayer
{
    /// <typeparam name="T1">~/Model/%Request.cs</typeparam>
    /// <typeparam name="T2">~/Model/%Answer.cs</typeparam>
    /// <typeparam name="T3">~/ViewModel/%ListViewModel</typeparam>
    public interface IReqAnsEngine<T1,T2,T3>
    {

        /// <summary>
        /// Interface method.
        /// Fill T3 with <b>@requests</b> and <b>@answers</b>.
        /// T3 is the combination of <b>requests</b> and <b>answers</b>.
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        T3 fillListViewModel(List<T1> requests, List<T2> answers);

    }
}