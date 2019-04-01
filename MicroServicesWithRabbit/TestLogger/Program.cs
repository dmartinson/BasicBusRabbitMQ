using log4net;


namespace TestLogger
{
    class Program
    {
        private static readonly ILog Log =
              LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            
            Log.Info("prueba logs Diego");
        }
    }
}
