using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TasksControl.Interfaces;
using TasksControl.Interfaces.Services;

namespace TasksControl.Services
{
    public class Engine : IEngineService
    {
        /// <summary>
        /// Classe para executar as ações do motor, o construtor pode ser refatorado para receber objetos necessários na execução da tarefa
        /// </summary>
        public Engine()
        { }
               
        /// <summary>
        /// Metodo que irá executar as ações programadas na Engine  
        /// </summary>
        public void Execute()
        {
            while (true)
            {                
                Thread.Sleep(1);
            }
        }
    }
}
