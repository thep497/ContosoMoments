﻿using ContosoMoments.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContosoMoments.ViewModels
{
    public class AlbumsListViewModel : BaseViewModel
    {
        //IMobileServiceTable<Album> albumTable;
        //IMobileServiceTable<User> userTable;

        public AlbumsListViewModel(MobileServiceClient client)
        {
            _client = client;
        }

        private string _ErrorMessage = null;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private bool _IsPending;
        public bool IsPending
        {
            get { return _IsPending; }
            set
            {
                _IsPending = value;
                OnPropertyChanged("IsPending");
            }
        }

        // View model properties
        private MobileServiceCollection<Album, Album> _Albums;
        public MobileServiceCollection<Album, Album> Albums
        {
            get { return _Albums; }
            set
            {
                _Albums = value;
                OnPropertyChanged("Albums");
            }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public async Task GetUserAsync(Guid userId)
        {
            try
            {
                //userTable = _client.GetTable<User>();
                //var user = await userTable.LookupAsync(userId);
                var user = await (App.Current as App).userTableSync.LookupAsync(userId.ToString());

                if (null != user)
                    User = user;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
        }

        public async Task GetAlbumsAsync()
        {
            IsPending = true;
            ErrorMessage = null;

            try
            {
                //albumTable = _client.GetTable<Album>();
                //Albums = await albumTable.ToCollectionAsync();
                Albums = await (App.Current as App).albumTableSync.ToCollectionAsync();                
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (HttpRequestException ex2)
            {
                ErrorMessage = ex2.Message;
            }
            finally
            {
                IsPending = false;
            }
        }

        public async Task<bool> AddNewAlbumAsync(string albumName)
        {
            bool bRes = true; //Assume success

            try
            {
                //await albumTable.InsertAsync(new Album() { AlbumName = albumName, IsDefault = false, UserId = User.UserId.ToString() });
                await (App.Current as App).albumTableSync.InsertAsync(new Album() { AlbumName = albumName, IsDefault = false, UserId = User.UserId.ToString() });
            }
            catch (Exception ex)
            {
                bRes = false;
            }

            return bRes;
        }

        public async Task<bool> DeleteAlbumAsync(Album selectedAlbum)
        {
            bool bRes = true; //Assume success

            try
            {
                //await albumTable.DeleteAsync(selectedAlbum);
                await (App.Current as App).albumTableSync.DeleteAsync(selectedAlbum);
            }
            catch (Exception ex)
            {
                bRes = false;
            }

            return bRes;
        }
    }
}
