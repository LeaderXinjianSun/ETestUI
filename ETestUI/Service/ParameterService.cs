using ETestUI.Common.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Service
{
    public class ParameterService : IParameterService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Param MyParam { get; set; }

        public bool Add(string name)
        {
            try
            {
                Project project = new Project();
                project.Name = name;
                if (MyParam.Projects.Count == 0)
                {
                    project.Id = 0;
                }
                else
                    project.Id = MyParam.Projects.Max(t => t.Id) + 1;
                project.Create = DateTime.Now;
                project.Modify = DateTime.Now;


                string jsonString = File.ReadAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CardConfig.json"));
                var cardConfig = JsonConvert.DeserializeObject<CardConfig>(jsonString);
                int k = 1;
                for (int i = 0; i < cardConfig.Cards.Count; i++)
                {
                    for (int j = 0; j < cardConfig.Cards[i]; j++)
                    {
                        project.TestPoints.Add(new TestPoint()
                        {
                            Index = k++,
                            Name = $"卡{i + 1}通道{j + 1}"
                        });
                    }
                }
                MyParam.Projects.Add(project);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool Load(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                MyParam = JsonConvert.DeserializeObject<Param>(jsonString);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public bool Save(string path)
        {
            try
            {
                MyParam.Projects[MyParam.SelectedIndex].Modify = DateTime.Now;
                string jsonString = JsonConvert.SerializeObject(MyParam, Formatting.Indented);
                File.WriteAllText(path, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }
    }
}
