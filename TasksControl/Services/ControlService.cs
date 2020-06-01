
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TasksControl.Entities;
using TasksControl.Interfaces;
using TasksControl.Interfaces.Services;
using TasksControl.Services;

namespace OCNSIO2.Domain.Processing.Interfaces.Services
{
    public class ControlService : IControlService
    {

        private List<FTask> _tasks;

        public ControlService()
        {
            _tasks = new List<FTask>();
        }

        /// <summary>
        /// Inicializador das tasks. Podem ser criadas N tasks de variados engines
        /// </summary>
        /// <returns></returns>
        public string Init()
        {
            var ret = "";
            try
            {
                var cancellSrc = new CancellationTokenSource();
                CancellationToken token = cancellSrc.Token;
                var engine = new Engine();
                var task = new FTask(new Action(engine.Execute), TaskCreationOptions.LongRunning, "name");
                task.Engine = engine;
                task.CancellationSource = cancellSrc;
                _tasks.Add(task);
               

                ret += DateTime.Now.ToLongTimeString().ToString() + ": Tarefas carregadas." + Environment.NewLine;

                if (StartTasks())
                    ret+= DateTime.Now.ToLongTimeString().ToString() + ": Todas as tarefas estão iniciadas."+Environment.NewLine;
                else
                {
                    ret+= DateTime.Now.ToLongTimeString().ToString() + ": Uma ou mais tarefas não foram iniciadas corretamente. Tente abrir o sistema novamente." + Environment.NewLine;
                    return ret;
                }

                return ret;
            }
            catch (Exception e)
            {
                return ret += DateTime.Now.ToLongTimeString().ToString() + "Falha na inicialização de tarefas: " +e.Message + Environment.NewLine; 
            }
        }

        public string Restart(string urlService)
        {
            if (StopTasks())
                return DateTime.Now.ToLongTimeString().ToString() + ": Tarefas canceladas, iniciando novo carregamento ..."+ 
                    Environment.NewLine + Init();
            else
                return "O sistema encontrou uma falha no cancelamento das tarefas!";

        }

        public IList<string> Monitoring()
        {
            return GetStatusTasks();
        }

        private bool StartTasks()
        {
            try
            {
                foreach (var t in _tasks)
                {
                    if (!t.CancellationSource.Token.IsCancellationRequested)
                        t.Start();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private IList<string> GetStatusTasks()
        {
            IList<string> statusTasks = new List<string>();
            var count = 0;
            foreach (var t in _tasks)
            {
                statusTasks.Add(new StringBuilder().AppendFormat("{3} : Tarefa: {0} - {1} - {2} "
                                                                , t.Id
                                                                , t.Name
                                                                , t.Status
                                                                , DateTime.Now.ToLongTimeString().ToString()).ToString());
                if (t.IsFaulted)
                    statusTasks.Add(new StringBuilder().AppendFormat(t.GetException()).ToString());

            }
            statusTasks.Add(DateTime.Now.ToLongTimeString().ToString() + ": Número de tarefas: " + _tasks.Count);
            statusTasks.Add(DateTime.Now.ToLongTimeString().ToString() + ": Tarefas com erro: " + _tasks.FindAll(x => x.IsFaulted).Count);
            statusTasks.Add(DateTime.Now.ToLongTimeString().ToString() + ": Total de arquivos: " + count +Environment.NewLine+Environment.NewLine);
            ClearListTask();
            return statusTasks;
        }
      
        private bool StopTasks()
        {
            try
            {
                foreach (var t in _tasks)
                {
                    t.CancellationSource.Cancel();
                }
                return true;
             }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ClearListTask()
        {
            _tasks.RemoveAll(x => x.IsFaulted);            
        }
    }
}
