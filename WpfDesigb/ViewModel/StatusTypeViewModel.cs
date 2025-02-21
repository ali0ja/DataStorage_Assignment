using Data.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDesigb.ViewModel
{
    public class StatusTypeViewModel
    {
        private readonly StatusTypeRepository _statusTypeRepository;
        public ObservableCollection<StatusTypeEntity> Statuses { get; set; }

        public StatusTypeViewModel(StatusTypeRepository statusTypeRepository)
        {
            _statusTypeRepository = statusTypeRepository;
            Statuses = new ObservableCollection<StatusTypeEntity>();
            LoadStatuses();
        }

        private async void LoadStatuses()
        {
            var statuses = await _statusTypeRepository.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Statuses.Clear();
                foreach (var status in statuses)
                {
                    Statuses.Add(status);
                }
            });
        }
    }
}
