using BeautifulMusic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace BeautifulMusic
{
    public static class SqlDataAccess
    {
        public static Func<SqlConnection> ConnectionFactory = () =>
        {
            var conn = new SqlConnection(getConnectionString());
            conn.Open();
            return conn;
        };

        private static string getConnectionString() => ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public static UserModel AddUser(UserModel user)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.UserTableAdapter { Connection = conn };
                var found = (int)adapter.CheckByLogin(user.Login);
                if(found > 0)
                    throw new DuplicateWaitObjectException("Пользователь с таким именем уже существует");
                var row = MusicDataSet.User.NewUserRow();
                row.Login = user.Login;
                row.Password = user.Password;
                MusicDataSet.User.AddUserRow(row);
                adapter.Update(row);
                user.Id = row.Id;
                return user;
            }
        }

        #region DataTable to a List{T}
        public static List<TSource> DataTableToList<TSource>(this DataTable dataTable)
            where TSource : new()
        {
            try
            {
                var dataList = new List<TSource>();
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
                var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                    select new
                    {
                        aProp.Name,
                        Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                    }).ToList();
                var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                    select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
                var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

                foreach(DataRow dataRow in dataTable.AsEnumerable().ToList())
                {
                    var aTSource = new TSource();
                    foreach(var aField in commonFields)
                    {
                        PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                        var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name]; //if database field is nullable
                        propertyInfos.SetValue(aTSource, value, null);
                    }
                    dataList.Add(aTSource);
                }
                return dataList;
            } catch
            {
                return null;
            }
        }
        #endregion

        public static void DeletePlayList(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.PlaylistTableAdapter { Connection = conn };
                adapter.Delete(id);
            }
        }

        public static List<AlbumModel> GetAlbums()
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.AlbumTableAdapter { Connection = conn };
                adapter.Fill(MusicDataSet.Album);
                return MusicDataSet.Album.DataTableToList<AlbumModel>();
            }
        }

        public static List<AlbumModel> GetAlbumsByArtist(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.AlbumTableAdapter { Connection = conn };
                adapter.FillByArtist(MusicDataSet.Album, id);
                return MusicDataSet.Album.DataTableToList<AlbumModel>();
            }
        }

        public static List<AlbumModel> GetAlbumsByGenre(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.AlbumTableAdapter { Connection = conn };
                adapter.FillByGenre(MusicDataSet.Album, id);
                return MusicDataSet.Album.DataTableToList<AlbumModel>();
            }
        }

        public static List<ArtistModel> GetArtist()
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.ArtistTableAdapter { Connection = conn };
                adapter.Fill(MusicDataSet.Artist);
                return MusicDataSet.Artist.DataTableToList<ArtistModel>();
            }
        }

        public static List<ArtistModel> GetArtistByGenre(int genreId)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.ArtistTableAdapter { Connection = conn };
                adapter.FillByGenre(MusicDataSet.Artist, genreId);
                return MusicDataSet.Artist.DataTableToList<ArtistModel>();
            }
        }

        public static List<GenreModel> GetGenres()
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.GenreTableAdapter { Connection = conn };
                adapter.Fill(MusicDataSet.Genre);
                return MusicDataSet.Genre.DataTableToList<GenreModel>();
            }
        }

        public static ObservableCollection<PlaylistModel> GetPlayLists()
        {
            var models = new ObservableCollection<PlaylistModel>();
            if(CurrentUser == null)
                return models;
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.PlaylistTableAdapter { Connection = conn };
                adapter.FillByUser(MusicDataSet.Playlist, CurrentUser.Id);
                MusicDataSet.Playlist.DataTableToList<PlaylistModel>().ForEach(x => models.Add(x));
                return models;
            }
        }

        public static List<TrackModel> GetTracks()
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.Fill(MusicDataSet.Track);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static List<TrackModel> GetTracksByAlbum(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.FillByAlbum(MusicDataSet.Track, id);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static List<TrackModel> GetTracksByArtist(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.FillByArtist(MusicDataSet.Track, id);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static List<TrackModel> GetTracksByGenre(int id)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.FillByPlayList(MusicDataSet.Track, id);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static ObservableCollection<TrackModel> GetTracksByPlayList(int id)
        {
            var tracks = new ObservableCollection<TrackModel>();
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.FillByPlayList(MusicDataSet.Track, id);
                MusicDataSet.Track.DataTableToList<TrackModel>().ForEach(x => tracks.Add(x));
                return tracks;
            }
        }

        public static List<TrackModel> GetTraks()
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.Fill(MusicDataSet.Track);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static bool IsUserByPaswordFound(UserModel user)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.UserTableAdapter { Connection = conn };
                var id = adapter.GetUserByPassword(user.Login, user.Password);
                if(id != null)
                    user.Id = (int)id;
                return user.Id > 0;
            }
        }

        public static void SaveNewPlayList(PlaylistModel model)
        {
            if(CurrentUser == null)
                return;
            string name = model.Name;
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.PlaylistTableAdapter { Connection = conn };
                List<TrackModel> tracks = new List<TrackModel>();
                var listId = adapter.GetPlayListByName(name);
                int.TryParse(listId + "", out int id);
                if(id > 0)
                {
                    var trackTableAdapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter
                    {
                        Connection = conn
                    };
                    trackTableAdapter.FillByPlayList(MusicDataSet.Track, id);
                    tracks = MusicDataSet.Track.DataTableToList<TrackModel>();
                } else
                {
                    var row = MusicDataSet.Playlist.NewPlaylistRow();
                    row.Name = name;
                    row.UserId = CurrentUser.Id;
                    MusicDataSet.Playlist.AddPlaylistRow(row);
                    adapter.Update(row);
                    id = row.Id;
                }
                model.Id = id;
                TrackModels.Select(x => x.Id)
                    .Except(tracks.Select(x => x.Id))
                    .Distinct()
                    .ToList()
                    .ForEach(
                        x =>
                        {
                            var row = MusicDataSet.PlaylistTrack.NewPlaylistTrackRow();
                            row.PlaylistId = id;
                            row.TrackId = x;
                            MusicDataSet.PlaylistTrack.AddPlaylistTrackRow(row);
                        });
                var playlistTrackTableAdapter = new BeautifulMusicDataSetTableAdapters.PlaylistTrackTableAdapter
                {
                    Connection = conn
                };
                playlistTrackTableAdapter.Update(MusicDataSet.PlaylistTrack);
            }
        }

        public static List<TrackModel> SearchTracs(string search)
        {
            using(var conn = ConnectionFactory())
            {
                var adapter = new BeautifulMusicDataSetTableAdapters.TrackTableAdapter { Connection = conn };
                adapter.FillBySearch(MusicDataSet.Track, search);
                return MusicDataSet.Track.DataTableToList<TrackModel>();
            }
        }

        public static UserModel CurrentUser { get; set; }

        public static BeautifulMusicDataSet MusicDataSet { get; set; } = new BeautifulMusicDataSet();

        public static List<TrackModel> MyTrackModels { get; set; } = new List<TrackModel>();

        public static ObservableCollection<PlaylistModel> Playlists { get; set; }

        public static TrackModel SelectedTrack { get; set; }

        public static ObservableCollection<TrackModel> TrackModels { get; set; }

        #region List{T} to a DataTable

        public static DataTable ToDataTable<T>(this IEnumerable<T> items, string name = "")
        {
            if(name == string.Empty)
                name = typeof(T).Name;
            var tb = new DataTable(name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if(!props.Any())
            {
                var t = typeof(T).Name;
                tb.Columns.Add(t, typeof(T));
                foreach(T item in items)
                {
                    tb.Rows.Add(item);
                }
            } else
            {
                foreach(PropertyInfo prop in props)
                {
                    Type t = GetCoreType(prop.PropertyType);
                    if(t.Name.Contains("List"))
                        continue;
                    tb.Columns.Add(prop.Name, t);
                    var c = (DisplayNameAttribute)prop.GetCustomAttribute(typeof(DisplayNameAttribute));
                    if(c != null)
                        tb.Columns[tb.Columns.Count - 1].Caption = c.DisplayName;
                }

                foreach(T item in items)
                {
                    var values = new object[tb.Columns.Count];

                    for(int i = 0; i < tb.Columns.Count; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }

                    tb.Rows.Add(values);
                }
            }
            return tb;
        }

        public static bool IsNullable(Type t)
        { return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)); }

        public static Type GetCoreType(Type t)
        {
            if(t != null && IsNullable(t))
            {
                if(!t.IsValueType)
                {
                    return t;
                } else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            } else
            {
                return t;
            }
        }
    }
    #endregion
}