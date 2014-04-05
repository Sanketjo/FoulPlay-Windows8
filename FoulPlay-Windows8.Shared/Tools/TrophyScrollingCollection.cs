﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using FoulPlay_Windows8.Annotations;
using Foulplay_Windows8.Core.Entities;
using Foulplay_Windows8.Core.Managers;

namespace FoulPlay_Windows8.Tools
{
    public class TrophyScrollingCollection : ObservableCollection<TrophyEntity.TrophyTitle>, ISupportIncrementalLoading, INotifyPropertyChanged
    {
        public int Offset;
        public UserAccountEntity UserAccountEntity;
        private bool _isLoading;

        public TrophyScrollingCollection()
        {
            HasMoreItems = true;
            IsLoading = false;
        }

        public string Username { get; set; }
        public int MaxCount { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        private async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            if (!IsLoading)
            {
                await LoadTrophies(Username);
            }
            var ret = new LoadMoreItemsResult {Count = count};
            return ret;
        }

        public new event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadTrophies(string username)
        {
            Offset = Offset + MaxCount;
            IsLoading = true;
            var trophyManager = new TrophyManager();
            TrophyEntity trophyList = await trophyManager.GetTrophyList(username, Offset, UserAccountEntity);
            if (trophyList == null)
            {
                //HasMoreItems = false;
                return false;
            }
            foreach (TrophyEntity.TrophyTitle trophy in trophyList.TrophyTitles)
            {
                Add(trophy);
            }
            if (trophyList.TrophyTitles.Any())
            {
                HasMoreItems = true;
                MaxCount += 64;
            }
            else
            {
                HasMoreItems = false;
            }
            IsLoading = false;
            return true;
        }
    }
}