using System.Linq;
using Android.Runtime;
using Java.Lang;
using Java.IO;


namespace App1
{
    public class ShellUtil
    {
        public static string RunCommand(string command)
        {
            Process process = null;
            var result = "false";
            try
            {
                process = Runtime.GetRuntime().Exec(command);
                process.WaitFor();
                result = InputStreamToString(new InputStreamAdapter(process.InputStream));
            }
            catch
            {
                return result;
            }
            finally
            {
                process?.Dispose();
            }
            return result;
        }


        public static string RunRootCmd(string command)
        {
            return RunRootCmd(command, ';');
        }

        public static string RunRootCmd(string command, char split)
        {
            Process process = null;
            var result = "false";
            try
            {
                process = Runtime.GetRuntime().Exec("su");
                var outputStream = process.OutputStream;
                var dataOutputStream = new DataOutputStream(outputStream);
                var cmds = command.Split(split);
                var temp = cmds.Aggregate("", (current, t) => current + (t + "\n"));
                dataOutputStream.WriteBytes(temp);
                dataOutputStream.Flush();
                dataOutputStream.WriteBytes("exit\n");
                dataOutputStream.Flush();
                process.WaitFor();
                result = InputStreamToString(new InputStreamAdapter(process.InputStream));
            }
            catch
            {
                return result;
            }
            finally
            {
                process?.Destroy();
            }
            return result;
        }

        private static string InputStreamToString(InputStream i)
        {
            var strBuff = new System.Text.StringBuilder();
            var b = new byte[1024];
            for (int n; (n = i.Read(b)) != -1;)
                strBuff.Append(new String(b, 0, n));
            return strBuff.ToString();
        }
    }
}