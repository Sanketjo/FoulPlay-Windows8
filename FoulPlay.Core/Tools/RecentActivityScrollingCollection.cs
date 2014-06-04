﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using FoulPlay.Core.Annotations;
using Foulplay_Windows8.Core.Entities;
using Foulplay_Windows8.Core.Managers;

namespace FoulPlay.Core.Tools
{
    public class RecentActivityScrollingCollection : ObservableCollection<RecentActivityEntity.Feed>,
        ISupportIncrementalLoading, INotifyPropertyChanged
    {
        public bool IsNews;
        public bool StorePromo;
        public UserAccountEntity UserAccountEntity;
        private bool _isEmpty;
        private bool _isLoading;

        public RecentActivityScrollingCollection()
        {
            HasMoreItems = true;
            IsLoading = false;
        }

        public string Username { get; set; }

        public int PageCount { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public bool IsEmpty
        {
            get { return _isEmpty; }

            private set
            {
                _isEmpty = value;
                NotifyPropertyChanged("IsEmpty");
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public bool HasMoreItems { get; private set; }

        private async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            if (!IsLoading)
            {
                LoadFeedList(Username);
            }
            var ret = new LoadMoreItemsResult {Count = count};
            return ret;
        }

        public async void LoadFeedList(string username)
        {
            IsLoading = true;
            var recentActivityManager = new RecentActivityManager();
            RecentActivityEntity feedEntity =
                await recentActivityManager.GetActivityFeed(username, PageCount, StorePromo, IsNews, UserAccountEntity);
            if (feedEntity == null)
            {
                HasMoreItems = false;
                IsLoading = false;
                return;
            }
            if (feedEntity.feed == null)
            {
                HasMoreItems = false;
                IsLoading = false;
                return;
            }
            foreach (RecentActivityEntity.Feed feed in feedEntity.feed)
            {
                Add(feed);
            }
            if (feedEntity.feed.Any())
            {
                HasMoreItems = true;
                PageCount++;
            }
            else
            {
                if (Count <= 0)
                {
                    IsEmpty = true;
                }
                HasMoreItems = false;
            }
            IsLoading = false;
        }

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
    }
}