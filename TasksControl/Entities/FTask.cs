using OCNSIO2.Domain.Processing.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using TasksControl.Interfaces;
using TasksControl.Interfaces.Services;

namespace TasksControl.Entities
{
    public class FTask : Task
    {

        private string _name;
        private IEngineService _engine;
        private CancellationTokenSource _cancellationSource;
       
        public FTask(Action action, string name):base(action)
        {
            _name = name;           
        }

        public FTask(Action action, TaskCreationOptions creationOptions, string name) : base(action, creationOptions)
        {
            _name = name;
        }

        public FTask(Action action, CancellationToken token, TaskCreationOptions creationOptions, string name) 
            : base(action:action, cancellationToken:token, creationOptions: creationOptions)
        {
            _name = name;
        }
           
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        internal IEngineService Engine { get => _engine; set => _engine = value; }
        public CancellationTokenSource CancellationSource { get => _cancellationSource; set => _cancellationSource = value; }

        public string GetException()
        {
            return base.Exception != null ? Exception.Message : "Falha não encontrada" ;
        }
    }
}
