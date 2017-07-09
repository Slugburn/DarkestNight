using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Slugburn.DarkestNight.Rules.Models;
using Slugburn.DarkestNight.Wpf.Annotations;

namespace Slugburn.DarkestNight.Wpf.ViewModels
{
    public class ConflictTargetVm 
    {
        public ConflictTargetVm(ConflictTargetModel model)
        {
            Id = model.Id;
            Name = model.Name;
            TargetNumber = model.TargetNumber;
            ResultNumber = model.ResultNumber;
            OutcomeDescription = model.OutcomeDescription;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TargetNumber { get; set; }
        public int ResultNumber { get; set; }
        public string OutcomeDescription { get; set; }

        public static List<ConflictTargetVm> Create(List<ConflictTargetModel> models)
        {
            return models?.Select(model => new ConflictTargetVm(model)).ToList();
        }
    }
}