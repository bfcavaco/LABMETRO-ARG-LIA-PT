using System;
using System.Diagnostics;
using System.Threading;

namespace LabMetro.GERAL
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class clsExecute
	{
		private string m_sResult;
		Process m_oProc;

		public clsExecute()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Run(string sFilePath, string sArgs, string sInput, string sWorkingDir, int nWaitTime)
		{
			m_sResult = "";
			m_oProc = new Process();
			ProcessStartInfo oInfo;
			if(sArgs==null||sArgs=="") oInfo = new ProcessStartInfo(sFilePath);
			else oInfo = new ProcessStartInfo(sFilePath, sArgs);
			if(sWorkingDir!=null&&sWorkingDir!="")
			{
				oInfo.WorkingDirectory = sWorkingDir;
			}
			oInfo.UseShellExecute = false;
			oInfo.RedirectStandardOutput = true;
			oInfo.RedirectStandardError = true;
			if(sInput!=null&&sInput!="")
			{
				oInfo.RedirectStandardInput = true;
			}
			m_oProc.StartInfo = oInfo;
			if(m_oProc.Start())
			{
				if(sInput!=null&&sInput!="")
				{
					m_oProc.StandardInput.Write(sInput);
					m_oProc.StandardInput.Close();
				}
				Thread oThread1 = new Thread(new ThreadStart(GetError));
				Thread oThread2 = new Thread(new ThreadStart(GetOutput));
				oThread1.Start();
				oThread2.Start();
				int nTotal = 0;
				int nPause = 50;
				while(oThread1.IsAlive||oThread2.IsAlive)
				{
					Thread.Sleep(nPause);
					nTotal += nPause;
					if(nTotal>(nWaitTime>0?nWaitTime:(1000*60*1)))
					{
						if(oThread1.IsAlive) oThread1.Abort();
						if(oThread2.IsAlive) oThread2.Abort();
						break;
					}
				}
				if(m_oProc.HasExited==false) 
				{
					m_oProc.Kill();
					m_sResult = "\r\nError: Hung process terminated ...\r\n";
				}
				m_oProc.Close();
				m_oProc.Dispose();
				return m_sResult;
			}
			else return null;
		}
		private void GetError()
		{
			if(m_oProc!=null&&m_oProc.StandardError!=null)
			{
				string sError = "";
				lock(m_oProc.StandardError)
				{
					sError = m_oProc.StandardError.ReadToEnd();
				}
				if(sError!="")
				{
					lock(this)
					{
						m_sResult += "\r\nError:"+sError+"\r\n";
					}
				}
			}
		}
		private void GetOutput()
		{
			if(m_oProc!=null&&m_oProc.StandardOutput!=null)
			{
				string sOutput = "";
				lock(m_oProc.StandardOutput)
				{
					sOutput = m_oProc.StandardOutput.ReadToEnd();
				}
				if(sOutput!="")
				{
					lock(this)
					{
						m_sResult += "\r\n"+sOutput;
					}
				}
			}
		}
	}
}