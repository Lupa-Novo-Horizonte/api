using System;
using System.Collections.Generic;
using System.Text;

namespace TE.BE.City.Infra.CrossCutting
{
    public static class ExecptionHelper
    {
        public class ExceptionRepository : Exception {
            public ExceptionRepository() : base ()
            {
            }

            /// <summary>
            /// Create the exception with description
            /// </summary>
            /// <param name="message">Exception description</param>
            public ExceptionRepository(String message)
              : base("RepositoryException: " + message)
            {
            }

            /// <summary>
            /// Create the exception with description and inner cause
            /// </summary>
            /// <param name="message">Exception description</param>
            /// <param name="innerException">Exception inner cause</param>
            public ExceptionRepository(String message, Exception innerException)
              : base("RepositoryException: " + message, innerException)
            {
            }
        }



        public class ExceptionService : Exception
        {
            public ExceptionService() : base()
            {
            }

            /// <summary>
            /// Create the exception with description
            /// </summary>
            /// <param name="message">Exception description</param>
            public ExceptionService(String message)
              : base("ServiceException: " + message)
            {
            }

            /// <summary>
            /// Create the exception with description and inner cause
            /// </summary>
            /// <param name="message">Exception description</param>
            /// <param name="innerException">Exception inner cause</param>
            public ExceptionService(String message, Exception innerException)
              : base("ServiceException: " + message, innerException)
            {
            }
        }
    }
}
