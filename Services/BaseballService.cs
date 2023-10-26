using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using TestingRadzenBlazorApp.Data;

namespace TestingRadzenBlazorApp
{
    public partial class BaseballService
    {
        BaseballContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly BaseballContext context;
        private readonly NavigationManager navigationManager;

        public BaseballService(BaseballContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAllstarfullsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/allstarfulls/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/allstarfulls/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAllstarfullsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/allstarfulls/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/allstarfulls/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAllstarfullsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Allstarfull>> GetAllstarfulls(Query query = null)
        {
            var items = Context.Allstarfulls.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAllstarfullsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAllstarfullGet(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);
        partial void OnGetAllstarfullByPlayerIdAndYearIdAndGameNum(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> GetAllstarfullByPlayerIdAndYearIdAndGameNum(string playerid, int yearid, int gamenum)
        {
            var items = Context.Allstarfulls
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.gameNum == gamenum);

 
            OnGetAllstarfullByPlayerIdAndYearIdAndGameNum(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAllstarfullGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAllstarfullCreated(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);
        partial void OnAfterAllstarfullCreated(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> CreateAllstarfull(TestingRadzenBlazorApp.Models.Baseball.Allstarfull allstarfull)
        {
            OnAllstarfullCreated(allstarfull);

            var existingItem = Context.Allstarfulls
                              .Where(i => i.playerID == allstarfull.playerID && i.yearID == allstarfull.yearID && i.gameNum == allstarfull.gameNum)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Allstarfulls.Add(allstarfull);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(allstarfull).State = EntityState.Detached;
                throw;
            }

            OnAfterAllstarfullCreated(allstarfull);

            return allstarfull;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> CancelAllstarfullChanges(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAllstarfullUpdated(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);
        partial void OnAfterAllstarfullUpdated(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> UpdateAllstarfull(string playerid, int yearid, int gamenum, TestingRadzenBlazorApp.Models.Baseball.Allstarfull allstarfull)
        {
            OnAllstarfullUpdated(allstarfull);

            var itemToUpdate = Context.Allstarfulls
                              .Where(i => i.playerID == allstarfull.playerID && i.yearID == allstarfull.yearID && i.gameNum == allstarfull.gameNum)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(allstarfull);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAllstarfullUpdated(allstarfull);

            return allstarfull;
        }

        partial void OnAllstarfullDeleted(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);
        partial void OnAfterAllstarfullDeleted(TestingRadzenBlazorApp.Models.Baseball.Allstarfull item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> DeleteAllstarfull(string playerid, int yearid, int gamenum)
        {
            var itemToDelete = Context.Allstarfulls
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.gameNum == gamenum)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAllstarfullDeleted(itemToDelete);


            Context.Allstarfulls.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAllstarfullDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAppearancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/appearances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/appearances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAppearancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/appearances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/appearances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAppearancesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Appearance> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Appearance>> GetAppearances(Query query = null)
        {
            var items = Context.Appearances.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAppearancesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAppearanceGet(TestingRadzenBlazorApp.Models.Baseball.Appearance item);
        partial void OnGetAppearanceByYearIdAndTeamIdAndPlayerId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Appearance> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Appearance> GetAppearanceByYearIdAndTeamIdAndPlayerId(int yearid, string teamid, string playerid)
        {
            var items = Context.Appearances
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.playerID == playerid);

 
            OnGetAppearanceByYearIdAndTeamIdAndPlayerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAppearanceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAppearanceCreated(TestingRadzenBlazorApp.Models.Baseball.Appearance item);
        partial void OnAfterAppearanceCreated(TestingRadzenBlazorApp.Models.Baseball.Appearance item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Appearance> CreateAppearance(TestingRadzenBlazorApp.Models.Baseball.Appearance appearance)
        {
            OnAppearanceCreated(appearance);

            var existingItem = Context.Appearances
                              .Where(i => i.yearID == appearance.yearID && i.teamID == appearance.teamID && i.playerID == appearance.playerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Appearances.Add(appearance);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(appearance).State = EntityState.Detached;
                throw;
            }

            OnAfterAppearanceCreated(appearance);

            return appearance;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Appearance> CancelAppearanceChanges(TestingRadzenBlazorApp.Models.Baseball.Appearance item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAppearanceUpdated(TestingRadzenBlazorApp.Models.Baseball.Appearance item);
        partial void OnAfterAppearanceUpdated(TestingRadzenBlazorApp.Models.Baseball.Appearance item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Appearance> UpdateAppearance(int yearid, string teamid, string playerid, TestingRadzenBlazorApp.Models.Baseball.Appearance appearance)
        {
            OnAppearanceUpdated(appearance);

            var itemToUpdate = Context.Appearances
                              .Where(i => i.yearID == appearance.yearID && i.teamID == appearance.teamID && i.playerID == appearance.playerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(appearance);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAppearanceUpdated(appearance);

            return appearance;
        }

        partial void OnAppearanceDeleted(TestingRadzenBlazorApp.Models.Baseball.Appearance item);
        partial void OnAfterAppearanceDeleted(TestingRadzenBlazorApp.Models.Baseball.Appearance item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Appearance> DeleteAppearance(int yearid, string teamid, string playerid)
        {
            var itemToDelete = Context.Appearances
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.playerID == playerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAppearanceDeleted(itemToDelete);


            Context.Appearances.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAppearanceDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAwardsmanagersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsmanagers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsmanagers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAwardsmanagersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsmanagers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsmanagers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAwardsmanagersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager>> GetAwardsmanagers(Query query = null)
        {
            var items = Context.Awardsmanagers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAwardsmanagersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAwardsmanagerGet(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);
        partial void OnGetAwardsmanagerByManagerIdAndAwardIdAndYearIdAndLgId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> GetAwardsmanagerByManagerIdAndAwardIdAndYearIdAndLgId(string managerid, string awardid, int yearid, string lgid)
        {
            var items = Context.Awardsmanagers
                              .AsNoTracking()
                              .Where(i => i.managerID == managerid && i.awardID == awardid && i.yearID == yearid && i.lgID == lgid);

 
            OnGetAwardsmanagerByManagerIdAndAwardIdAndYearIdAndLgId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAwardsmanagerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAwardsmanagerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);
        partial void OnAfterAwardsmanagerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> CreateAwardsmanager(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager awardsmanager)
        {
            OnAwardsmanagerCreated(awardsmanager);

            var existingItem = Context.Awardsmanagers
                              .Where(i => i.managerID == awardsmanager.managerID && i.awardID == awardsmanager.awardID && i.yearID == awardsmanager.yearID && i.lgID == awardsmanager.lgID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Awardsmanagers.Add(awardsmanager);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(awardsmanager).State = EntityState.Detached;
                throw;
            }

            OnAfterAwardsmanagerCreated(awardsmanager);

            return awardsmanager;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> CancelAwardsmanagerChanges(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAwardsmanagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);
        partial void OnAfterAwardsmanagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> UpdateAwardsmanager(string managerid, string awardid, int yearid, string lgid, TestingRadzenBlazorApp.Models.Baseball.Awardsmanager awardsmanager)
        {
            OnAwardsmanagerUpdated(awardsmanager);

            var itemToUpdate = Context.Awardsmanagers
                              .Where(i => i.managerID == awardsmanager.managerID && i.awardID == awardsmanager.awardID && i.yearID == awardsmanager.yearID && i.lgID == awardsmanager.lgID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(awardsmanager);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAwardsmanagerUpdated(awardsmanager);

            return awardsmanager;
        }

        partial void OnAwardsmanagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);
        partial void OnAfterAwardsmanagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsmanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> DeleteAwardsmanager(string managerid, string awardid, int yearid, string lgid)
        {
            var itemToDelete = Context.Awardsmanagers
                              .Where(i => i.managerID == managerid && i.awardID == awardid && i.yearID == yearid && i.lgID == lgid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAwardsmanagerDeleted(itemToDelete);


            Context.Awardsmanagers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAwardsmanagerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAwardsplayersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAwardsplayersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAwardsplayersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer>> GetAwardsplayers(Query query = null)
        {
            var items = Context.Awardsplayers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAwardsplayersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAwardsplayerGet(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);
        partial void OnGetAwardsplayerByPlayerIdAndAwardIdAndYearIdAndLgId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> GetAwardsplayerByPlayerIdAndAwardIdAndYearIdAndLgId(string playerid, string awardid, int yearid, string lgid)
        {
            var items = Context.Awardsplayers
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.awardID == awardid && i.yearID == yearid && i.lgID == lgid);

 
            OnGetAwardsplayerByPlayerIdAndAwardIdAndYearIdAndLgId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAwardsplayerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAwardsplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);
        partial void OnAfterAwardsplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> CreateAwardsplayer(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer awardsplayer)
        {
            OnAwardsplayerCreated(awardsplayer);

            var existingItem = Context.Awardsplayers
                              .Where(i => i.playerID == awardsplayer.playerID && i.awardID == awardsplayer.awardID && i.yearID == awardsplayer.yearID && i.lgID == awardsplayer.lgID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Awardsplayers.Add(awardsplayer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(awardsplayer).State = EntityState.Detached;
                throw;
            }

            OnAfterAwardsplayerCreated(awardsplayer);

            return awardsplayer;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> CancelAwardsplayerChanges(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAwardsplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);
        partial void OnAfterAwardsplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> UpdateAwardsplayer(string playerid, string awardid, int yearid, string lgid, TestingRadzenBlazorApp.Models.Baseball.Awardsplayer awardsplayer)
        {
            OnAwardsplayerUpdated(awardsplayer);

            var itemToUpdate = Context.Awardsplayers
                              .Where(i => i.playerID == awardsplayer.playerID && i.awardID == awardsplayer.awardID && i.yearID == awardsplayer.yearID && i.lgID == awardsplayer.lgID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(awardsplayer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAwardsplayerUpdated(awardsplayer);

            return awardsplayer;
        }

        partial void OnAwardsplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);
        partial void OnAfterAwardsplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> DeleteAwardsplayer(string playerid, string awardid, int yearid, string lgid)
        {
            var itemToDelete = Context.Awardsplayers
                              .Where(i => i.playerID == playerid && i.awardID == awardid && i.yearID == yearid && i.lgID == lgid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAwardsplayerDeleted(itemToDelete);


            Context.Awardsplayers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAwardsplayerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAwardssharemanagersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardssharemanagers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardssharemanagers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAwardssharemanagersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardssharemanagers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardssharemanagers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAwardssharemanagersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager>> GetAwardssharemanagers(Query query = null)
        {
            var items = Context.Awardssharemanagers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAwardssharemanagersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAwardssharemanagerGet(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);
        partial void OnGetAwardssharemanagerByAwardIdAndYearIdAndLgIdAndManagerId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> GetAwardssharemanagerByAwardIdAndYearIdAndLgIdAndManagerId(string awardid, int yearid, string lgid, string managerid)
        {
            var items = Context.Awardssharemanagers
                              .AsNoTracking()
                              .Where(i => i.awardID == awardid && i.yearID == yearid && i.lgID == lgid && i.managerID == managerid);

 
            OnGetAwardssharemanagerByAwardIdAndYearIdAndLgIdAndManagerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAwardssharemanagerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAwardssharemanagerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);
        partial void OnAfterAwardssharemanagerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> CreateAwardssharemanager(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager awardssharemanager)
        {
            OnAwardssharemanagerCreated(awardssharemanager);

            var existingItem = Context.Awardssharemanagers
                              .Where(i => i.awardID == awardssharemanager.awardID && i.yearID == awardssharemanager.yearID && i.lgID == awardssharemanager.lgID && i.managerID == awardssharemanager.managerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Awardssharemanagers.Add(awardssharemanager);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(awardssharemanager).State = EntityState.Detached;
                throw;
            }

            OnAfterAwardssharemanagerCreated(awardssharemanager);

            return awardssharemanager;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> CancelAwardssharemanagerChanges(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAwardssharemanagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);
        partial void OnAfterAwardssharemanagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> UpdateAwardssharemanager(string awardid, int yearid, string lgid, string managerid, TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager awardssharemanager)
        {
            OnAwardssharemanagerUpdated(awardssharemanager);

            var itemToUpdate = Context.Awardssharemanagers
                              .Where(i => i.awardID == awardssharemanager.awardID && i.yearID == awardssharemanager.yearID && i.lgID == awardssharemanager.lgID && i.managerID == awardssharemanager.managerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(awardssharemanager);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAwardssharemanagerUpdated(awardssharemanager);

            return awardssharemanager;
        }

        partial void OnAwardssharemanagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);
        partial void OnAfterAwardssharemanagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> DeleteAwardssharemanager(string awardid, int yearid, string lgid, string managerid)
        {
            var itemToDelete = Context.Awardssharemanagers
                              .Where(i => i.awardID == awardid && i.yearID == yearid && i.lgID == lgid && i.managerID == managerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAwardssharemanagerDeleted(itemToDelete);


            Context.Awardssharemanagers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAwardssharemanagerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAwardsshareplayersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsshareplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsshareplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAwardsshareplayersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/awardsshareplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/awardsshareplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAwardsshareplayersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer>> GetAwardsshareplayers(Query query = null)
        {
            var items = Context.Awardsshareplayers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAwardsshareplayersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAwardsshareplayerGet(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);
        partial void OnGetAwardsshareplayerByAwardIdAndYearIdAndLgIdAndPlayerId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> GetAwardsshareplayerByAwardIdAndYearIdAndLgIdAndPlayerId(string awardid, int yearid, string lgid, string playerid)
        {
            var items = Context.Awardsshareplayers
                              .AsNoTracking()
                              .Where(i => i.awardID == awardid && i.yearID == yearid && i.lgID == lgid && i.playerID == playerid);

 
            OnGetAwardsshareplayerByAwardIdAndYearIdAndLgIdAndPlayerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAwardsshareplayerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAwardsshareplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);
        partial void OnAfterAwardsshareplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> CreateAwardsshareplayer(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer awardsshareplayer)
        {
            OnAwardsshareplayerCreated(awardsshareplayer);

            var existingItem = Context.Awardsshareplayers
                              .Where(i => i.awardID == awardsshareplayer.awardID && i.yearID == awardsshareplayer.yearID && i.lgID == awardsshareplayer.lgID && i.playerID == awardsshareplayer.playerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Awardsshareplayers.Add(awardsshareplayer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(awardsshareplayer).State = EntityState.Detached;
                throw;
            }

            OnAfterAwardsshareplayerCreated(awardsshareplayer);

            return awardsshareplayer;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> CancelAwardsshareplayerChanges(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAwardsshareplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);
        partial void OnAfterAwardsshareplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> UpdateAwardsshareplayer(string awardid, int yearid, string lgid, string playerid, TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer awardsshareplayer)
        {
            OnAwardsshareplayerUpdated(awardsshareplayer);

            var itemToUpdate = Context.Awardsshareplayers
                              .Where(i => i.awardID == awardsshareplayer.awardID && i.yearID == awardsshareplayer.yearID && i.lgID == awardsshareplayer.lgID && i.playerID == awardsshareplayer.playerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(awardsshareplayer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAwardsshareplayerUpdated(awardsshareplayer);

            return awardsshareplayer;
        }

        partial void OnAwardsshareplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);
        partial void OnAfterAwardsshareplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> DeleteAwardsshareplayer(string awardid, int yearid, string lgid, string playerid)
        {
            var itemToDelete = Context.Awardsshareplayers
                              .Where(i => i.awardID == awardid && i.yearID == yearid && i.lgID == lgid && i.playerID == playerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAwardsshareplayerDeleted(itemToDelete);


            Context.Awardsshareplayers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAwardsshareplayerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBattingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/battings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/battings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBattingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/battings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/battings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBattingsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Batting> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Batting>> GetBattings(Query query = null)
        {
            var items = Context.Battings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBattingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBattingGet(TestingRadzenBlazorApp.Models.Baseball.Batting item);
        partial void OnGetBattingByPlayerIdAndYearIdAndStint(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Batting> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Batting> GetBattingByPlayerIdAndYearIdAndStint(string playerid, int yearid, int stint)
        {
            var items = Context.Battings
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint);

 
            OnGetBattingByPlayerIdAndYearIdAndStint(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBattingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBattingCreated(TestingRadzenBlazorApp.Models.Baseball.Batting item);
        partial void OnAfterBattingCreated(TestingRadzenBlazorApp.Models.Baseball.Batting item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Batting> CreateBatting(TestingRadzenBlazorApp.Models.Baseball.Batting batting)
        {
            OnBattingCreated(batting);

            var existingItem = Context.Battings
                              .Where(i => i.playerID == batting.playerID && i.yearID == batting.yearID && i.stint == batting.stint)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Battings.Add(batting);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(batting).State = EntityState.Detached;
                throw;
            }

            OnAfterBattingCreated(batting);

            return batting;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Batting> CancelBattingChanges(TestingRadzenBlazorApp.Models.Baseball.Batting item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBattingUpdated(TestingRadzenBlazorApp.Models.Baseball.Batting item);
        partial void OnAfterBattingUpdated(TestingRadzenBlazorApp.Models.Baseball.Batting item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Batting> UpdateBatting(string playerid, int yearid, int stint, TestingRadzenBlazorApp.Models.Baseball.Batting batting)
        {
            OnBattingUpdated(batting);

            var itemToUpdate = Context.Battings
                              .Where(i => i.playerID == batting.playerID && i.yearID == batting.yearID && i.stint == batting.stint)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(batting);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBattingUpdated(batting);

            return batting;
        }

        partial void OnBattingDeleted(TestingRadzenBlazorApp.Models.Baseball.Batting item);
        partial void OnAfterBattingDeleted(TestingRadzenBlazorApp.Models.Baseball.Batting item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Batting> DeleteBatting(string playerid, int yearid, int stint)
        {
            var itemToDelete = Context.Battings
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBattingDeleted(itemToDelete);


            Context.Battings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBattingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBattingpostsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/battingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/battingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBattingpostsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/battingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/battingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBattingpostsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Battingpost> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Battingpost>> GetBattingposts(Query query = null)
        {
            var items = Context.Battingposts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBattingpostsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBattingpostGet(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);
        partial void OnGetBattingpostByYearIdAndRoundAndPlayerId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Battingpost> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Battingpost> GetBattingpostByYearIdAndRoundAndPlayerId(int yearid, string round, string playerid)
        {
            var items = Context.Battingposts
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.round == round && i.playerID == playerid);

 
            OnGetBattingpostByYearIdAndRoundAndPlayerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBattingpostGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBattingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);
        partial void OnAfterBattingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Battingpost> CreateBattingpost(TestingRadzenBlazorApp.Models.Baseball.Battingpost battingpost)
        {
            OnBattingpostCreated(battingpost);

            var existingItem = Context.Battingposts
                              .Where(i => i.yearID == battingpost.yearID && i.round == battingpost.round && i.playerID == battingpost.playerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Battingposts.Add(battingpost);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(battingpost).State = EntityState.Detached;
                throw;
            }

            OnAfterBattingpostCreated(battingpost);

            return battingpost;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Battingpost> CancelBattingpostChanges(TestingRadzenBlazorApp.Models.Baseball.Battingpost item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBattingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);
        partial void OnAfterBattingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Battingpost> UpdateBattingpost(int yearid, string round, string playerid, TestingRadzenBlazorApp.Models.Baseball.Battingpost battingpost)
        {
            OnBattingpostUpdated(battingpost);

            var itemToUpdate = Context.Battingposts
                              .Where(i => i.yearID == battingpost.yearID && i.round == battingpost.round && i.playerID == battingpost.playerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(battingpost);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBattingpostUpdated(battingpost);

            return battingpost;
        }

        partial void OnBattingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);
        partial void OnAfterBattingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Battingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Battingpost> DeleteBattingpost(int yearid, string round, string playerid)
        {
            var itemToDelete = Context.Battingposts
                              .Where(i => i.yearID == yearid && i.round == round && i.playerID == playerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBattingpostDeleted(itemToDelete);


            Context.Battingposts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBattingpostDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportElsTeamnamesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/elsteamnames/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/elsteamnames/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportElsTeamnamesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/elsteamnames/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/elsteamnames/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnElsTeamnamesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.ElsTeamname> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.ElsTeamname>> GetElsTeamnames(Query query = null)
        {
            var items = Context.ElsTeamnames.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnElsTeamnamesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportFieldingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFieldingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFieldingsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fielding> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fielding>> GetFieldings(Query query = null)
        {
            var items = Context.Fieldings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnFieldingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFieldingGet(TestingRadzenBlazorApp.Models.Baseball.Fielding item);
        partial void OnGetFieldingByPlayerIdAndYearIdAndStintAndPos(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fielding> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fielding> GetFieldingByPlayerIdAndYearIdAndStintAndPos(string playerid, int yearid, int stint, string pos)
        {
            var items = Context.Fieldings
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint && i.POS == pos);

 
            OnGetFieldingByPlayerIdAndYearIdAndStintAndPos(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFieldingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFieldingCreated(TestingRadzenBlazorApp.Models.Baseball.Fielding item);
        partial void OnAfterFieldingCreated(TestingRadzenBlazorApp.Models.Baseball.Fielding item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fielding> CreateFielding(TestingRadzenBlazorApp.Models.Baseball.Fielding fielding)
        {
            OnFieldingCreated(fielding);

            var existingItem = Context.Fieldings
                              .Where(i => i.playerID == fielding.playerID && i.yearID == fielding.yearID && i.stint == fielding.stint && i.POS == fielding.POS)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Fieldings.Add(fielding);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fielding).State = EntityState.Detached;
                throw;
            }

            OnAfterFieldingCreated(fielding);

            return fielding;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fielding> CancelFieldingChanges(TestingRadzenBlazorApp.Models.Baseball.Fielding item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFieldingUpdated(TestingRadzenBlazorApp.Models.Baseball.Fielding item);
        partial void OnAfterFieldingUpdated(TestingRadzenBlazorApp.Models.Baseball.Fielding item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fielding> UpdateFielding(string playerid, int yearid, int stint, string pos, TestingRadzenBlazorApp.Models.Baseball.Fielding fielding)
        {
            OnFieldingUpdated(fielding);

            var itemToUpdate = Context.Fieldings
                              .Where(i => i.playerID == fielding.playerID && i.yearID == fielding.yearID && i.stint == fielding.stint && i.POS == fielding.POS)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fielding);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFieldingUpdated(fielding);

            return fielding;
        }

        partial void OnFieldingDeleted(TestingRadzenBlazorApp.Models.Baseball.Fielding item);
        partial void OnAfterFieldingDeleted(TestingRadzenBlazorApp.Models.Baseball.Fielding item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fielding> DeleteFielding(string playerid, int yearid, int stint, string pos)
        {
            var itemToDelete = Context.Fieldings
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint && i.POS == pos)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFieldingDeleted(itemToDelete);


            Context.Fieldings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFieldingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportFieldingofsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldingofs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldingofs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFieldingofsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldingofs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldingofs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFieldingofsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingof>> GetFieldingofs(Query query = null)
        {
            var items = Context.Fieldingofs.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnFieldingofsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFieldingofGet(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);
        partial void OnGetFieldingofByPlayerIdAndYearIdAndStint(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> GetFieldingofByPlayerIdAndYearIdAndStint(string playerid, int yearid, int stint)
        {
            var items = Context.Fieldingofs
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint);

 
            OnGetFieldingofByPlayerIdAndYearIdAndStint(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFieldingofGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFieldingofCreated(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);
        partial void OnAfterFieldingofCreated(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> CreateFieldingof(TestingRadzenBlazorApp.Models.Baseball.Fieldingof fieldingof)
        {
            OnFieldingofCreated(fieldingof);

            var existingItem = Context.Fieldingofs
                              .Where(i => i.playerID == fieldingof.playerID && i.yearID == fieldingof.yearID && i.stint == fieldingof.stint)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Fieldingofs.Add(fieldingof);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fieldingof).State = EntityState.Detached;
                throw;
            }

            OnAfterFieldingofCreated(fieldingof);

            return fieldingof;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> CancelFieldingofChanges(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFieldingofUpdated(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);
        partial void OnAfterFieldingofUpdated(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> UpdateFieldingof(string playerid, int yearid, int stint, TestingRadzenBlazorApp.Models.Baseball.Fieldingof fieldingof)
        {
            OnFieldingofUpdated(fieldingof);

            var itemToUpdate = Context.Fieldingofs
                              .Where(i => i.playerID == fieldingof.playerID && i.yearID == fieldingof.yearID && i.stint == fieldingof.stint)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fieldingof);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFieldingofUpdated(fieldingof);

            return fieldingof;
        }

        partial void OnFieldingofDeleted(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);
        partial void OnAfterFieldingofDeleted(TestingRadzenBlazorApp.Models.Baseball.Fieldingof item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> DeleteFieldingof(string playerid, int yearid, int stint)
        {
            var itemToDelete = Context.Fieldingofs
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFieldingofDeleted(itemToDelete);


            Context.Fieldingofs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFieldingofDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportFieldingpostsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFieldingpostsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/fieldingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/fieldingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFieldingpostsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost>> GetFieldingposts(Query query = null)
        {
            var items = Context.Fieldingposts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnFieldingpostsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFieldingpostGet(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);
        partial void OnGetFieldingpostByPlayerIdAndYearIdAndRoundAndPos(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> GetFieldingpostByPlayerIdAndYearIdAndRoundAndPos(string playerid, int yearid, string round, string pos)
        {
            var items = Context.Fieldingposts
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.round == round && i.POS == pos);

 
            OnGetFieldingpostByPlayerIdAndYearIdAndRoundAndPos(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFieldingpostGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFieldingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);
        partial void OnAfterFieldingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> CreateFieldingpost(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost fieldingpost)
        {
            OnFieldingpostCreated(fieldingpost);

            var existingItem = Context.Fieldingposts
                              .Where(i => i.playerID == fieldingpost.playerID && i.yearID == fieldingpost.yearID && i.round == fieldingpost.round && i.POS == fieldingpost.POS)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Fieldingposts.Add(fieldingpost);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fieldingpost).State = EntityState.Detached;
                throw;
            }

            OnAfterFieldingpostCreated(fieldingpost);

            return fieldingpost;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> CancelFieldingpostChanges(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFieldingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);
        partial void OnAfterFieldingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> UpdateFieldingpost(string playerid, int yearid, string round, string pos, TestingRadzenBlazorApp.Models.Baseball.Fieldingpost fieldingpost)
        {
            OnFieldingpostUpdated(fieldingpost);

            var itemToUpdate = Context.Fieldingposts
                              .Where(i => i.playerID == fieldingpost.playerID && i.yearID == fieldingpost.yearID && i.round == fieldingpost.round && i.POS == fieldingpost.POS)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fieldingpost);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFieldingpostUpdated(fieldingpost);

            return fieldingpost;
        }

        partial void OnFieldingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);
        partial void OnAfterFieldingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Fieldingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> DeleteFieldingpost(string playerid, int yearid, string round, string pos)
        {
            var itemToDelete = Context.Fieldingposts
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.round == round && i.POS == pos)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFieldingpostDeleted(itemToDelete);


            Context.Fieldingposts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFieldingpostDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportHalloffamesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/halloffames/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/halloffames/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportHalloffamesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/halloffames/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/halloffames/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnHalloffamesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Halloffame> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Halloffame>> GetHalloffames(Query query = null)
        {
            var items = Context.Halloffames.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnHalloffamesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHalloffameGet(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);
        partial void OnGetHalloffameByHofIdAndYearId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Halloffame> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Halloffame> GetHalloffameByHofIdAndYearId(string hofid, int yearid)
        {
            var items = Context.Halloffames
                              .AsNoTracking()
                              .Where(i => i.hofID == hofid && i.yearID == yearid);

 
            OnGetHalloffameByHofIdAndYearId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnHalloffameGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnHalloffameCreated(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);
        partial void OnAfterHalloffameCreated(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Halloffame> CreateHalloffame(TestingRadzenBlazorApp.Models.Baseball.Halloffame halloffame)
        {
            OnHalloffameCreated(halloffame);

            var existingItem = Context.Halloffames
                              .Where(i => i.hofID == halloffame.hofID && i.yearID == halloffame.yearID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Halloffames.Add(halloffame);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(halloffame).State = EntityState.Detached;
                throw;
            }

            OnAfterHalloffameCreated(halloffame);

            return halloffame;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Halloffame> CancelHalloffameChanges(TestingRadzenBlazorApp.Models.Baseball.Halloffame item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnHalloffameUpdated(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);
        partial void OnAfterHalloffameUpdated(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Halloffame> UpdateHalloffame(string hofid, int yearid, TestingRadzenBlazorApp.Models.Baseball.Halloffame halloffame)
        {
            OnHalloffameUpdated(halloffame);

            var itemToUpdate = Context.Halloffames
                              .Where(i => i.hofID == halloffame.hofID && i.yearID == halloffame.yearID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(halloffame);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterHalloffameUpdated(halloffame);

            return halloffame;
        }

        partial void OnHalloffameDeleted(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);
        partial void OnAfterHalloffameDeleted(TestingRadzenBlazorApp.Models.Baseball.Halloffame item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Halloffame> DeleteHalloffame(string hofid, int yearid)
        {
            var itemToDelete = Context.Halloffames
                              .Where(i => i.hofID == hofid && i.yearID == yearid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnHalloffameDeleted(itemToDelete);


            Context.Halloffames.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterHalloffameDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportManagersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportManagersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnManagersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Manager> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Manager>> GetManagers(Query query = null)
        {
            var items = Context.Managers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnManagersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnManagerGet(TestingRadzenBlazorApp.Models.Baseball.Manager item);
        partial void OnGetManagerByYearIdAndTeamIdAndInseason(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Manager> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Manager> GetManagerByYearIdAndTeamIdAndInseason(int yearid, string teamid, int inseason)
        {
            var items = Context.Managers
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.inseason == inseason);

 
            OnGetManagerByYearIdAndTeamIdAndInseason(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnManagerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnManagerCreated(TestingRadzenBlazorApp.Models.Baseball.Manager item);
        partial void OnAfterManagerCreated(TestingRadzenBlazorApp.Models.Baseball.Manager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Manager> CreateManager(TestingRadzenBlazorApp.Models.Baseball.Manager manager)
        {
            OnManagerCreated(manager);

            var existingItem = Context.Managers
                              .Where(i => i.yearID == manager.yearID && i.teamID == manager.teamID && i.inseason == manager.inseason)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Managers.Add(manager);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(manager).State = EntityState.Detached;
                throw;
            }

            OnAfterManagerCreated(manager);

            return manager;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Manager> CancelManagerChanges(TestingRadzenBlazorApp.Models.Baseball.Manager item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnManagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Manager item);
        partial void OnAfterManagerUpdated(TestingRadzenBlazorApp.Models.Baseball.Manager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Manager> UpdateManager(int yearid, string teamid, int inseason, TestingRadzenBlazorApp.Models.Baseball.Manager manager)
        {
            OnManagerUpdated(manager);

            var itemToUpdate = Context.Managers
                              .Where(i => i.yearID == manager.yearID && i.teamID == manager.teamID && i.inseason == manager.inseason)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(manager);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterManagerUpdated(manager);

            return manager;
        }

        partial void OnManagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Manager item);
        partial void OnAfterManagerDeleted(TestingRadzenBlazorApp.Models.Baseball.Manager item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Manager> DeleteManager(int yearid, string teamid, int inseason)
        {
            var itemToDelete = Context.Managers
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.inseason == inseason)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnManagerDeleted(itemToDelete);


            Context.Managers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterManagerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportManagershalvesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/managershalves/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/managershalves/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportManagershalvesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/managershalves/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/managershalves/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnManagershalvesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Managershalf> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Managershalf>> GetManagershalves(Query query = null)
        {
            var items = Context.Managershalves.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnManagershalvesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnManagershalfGet(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);
        partial void OnGetManagershalfByManagerIdAndYearIdAndTeamIdAndHalf(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Managershalf> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Managershalf> GetManagershalfByManagerIdAndYearIdAndTeamIdAndHalf(string managerid, int yearid, string teamid, int half)
        {
            var items = Context.Managershalves
                              .AsNoTracking()
                              .Where(i => i.managerID == managerid && i.yearID == yearid && i.teamID == teamid && i.half == half);

 
            OnGetManagershalfByManagerIdAndYearIdAndTeamIdAndHalf(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnManagershalfGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnManagershalfCreated(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);
        partial void OnAfterManagershalfCreated(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Managershalf> CreateManagershalf(TestingRadzenBlazorApp.Models.Baseball.Managershalf managershalf)
        {
            OnManagershalfCreated(managershalf);

            var existingItem = Context.Managershalves
                              .Where(i => i.managerID == managershalf.managerID && i.yearID == managershalf.yearID && i.teamID == managershalf.teamID && i.half == managershalf.half)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Managershalves.Add(managershalf);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(managershalf).State = EntityState.Detached;
                throw;
            }

            OnAfterManagershalfCreated(managershalf);

            return managershalf;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Managershalf> CancelManagershalfChanges(TestingRadzenBlazorApp.Models.Baseball.Managershalf item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnManagershalfUpdated(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);
        partial void OnAfterManagershalfUpdated(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Managershalf> UpdateManagershalf(string managerid, int yearid, string teamid, int half, TestingRadzenBlazorApp.Models.Baseball.Managershalf managershalf)
        {
            OnManagershalfUpdated(managershalf);

            var itemToUpdate = Context.Managershalves
                              .Where(i => i.managerID == managershalf.managerID && i.yearID == managershalf.yearID && i.teamID == managershalf.teamID && i.half == managershalf.half)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(managershalf);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterManagershalfUpdated(managershalf);

            return managershalf;
        }

        partial void OnManagershalfDeleted(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);
        partial void OnAfterManagershalfDeleted(TestingRadzenBlazorApp.Models.Baseball.Managershalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Managershalf> DeleteManagershalf(string managerid, int yearid, string teamid, int half)
        {
            var itemToDelete = Context.Managershalves
                              .Where(i => i.managerID == managerid && i.yearID == yearid && i.teamID == teamid && i.half == half)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnManagershalfDeleted(itemToDelete);


            Context.Managershalves.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterManagershalfDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPitchingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/pitchings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/pitchings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPitchingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/pitchings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/pitchings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPitchingsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitching> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitching>> GetPitchings(Query query = null)
        {
            var items = Context.Pitchings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPitchingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPitchingGet(TestingRadzenBlazorApp.Models.Baseball.Pitching item);
        partial void OnGetPitchingByPlayerIdAndYearIdAndStint(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitching> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitching> GetPitchingByPlayerIdAndYearIdAndStint(string playerid, int yearid, int stint)
        {
            var items = Context.Pitchings
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint);

 
            OnGetPitchingByPlayerIdAndYearIdAndStint(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPitchingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPitchingCreated(TestingRadzenBlazorApp.Models.Baseball.Pitching item);
        partial void OnAfterPitchingCreated(TestingRadzenBlazorApp.Models.Baseball.Pitching item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitching> CreatePitching(TestingRadzenBlazorApp.Models.Baseball.Pitching pitching)
        {
            OnPitchingCreated(pitching);

            var existingItem = Context.Pitchings
                              .Where(i => i.playerID == pitching.playerID && i.yearID == pitching.yearID && i.stint == pitching.stint)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Pitchings.Add(pitching);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(pitching).State = EntityState.Detached;
                throw;
            }

            OnAfterPitchingCreated(pitching);

            return pitching;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitching> CancelPitchingChanges(TestingRadzenBlazorApp.Models.Baseball.Pitching item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPitchingUpdated(TestingRadzenBlazorApp.Models.Baseball.Pitching item);
        partial void OnAfterPitchingUpdated(TestingRadzenBlazorApp.Models.Baseball.Pitching item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitching> UpdatePitching(string playerid, int yearid, int stint, TestingRadzenBlazorApp.Models.Baseball.Pitching pitching)
        {
            OnPitchingUpdated(pitching);

            var itemToUpdate = Context.Pitchings
                              .Where(i => i.playerID == pitching.playerID && i.yearID == pitching.yearID && i.stint == pitching.stint)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(pitching);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPitchingUpdated(pitching);

            return pitching;
        }

        partial void OnPitchingDeleted(TestingRadzenBlazorApp.Models.Baseball.Pitching item);
        partial void OnAfterPitchingDeleted(TestingRadzenBlazorApp.Models.Baseball.Pitching item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitching> DeletePitching(string playerid, int yearid, int stint)
        {
            var itemToDelete = Context.Pitchings
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.stint == stint)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPitchingDeleted(itemToDelete);


            Context.Pitchings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPitchingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPitchingpostsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/pitchingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/pitchingposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPitchingpostsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/pitchingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/pitchingposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPitchingpostsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost>> GetPitchingposts(Query query = null)
        {
            var items = Context.Pitchingposts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPitchingpostsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPitchingpostGet(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);
        partial void OnGetPitchingpostByPlayerIdAndYearIdAndRound(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> GetPitchingpostByPlayerIdAndYearIdAndRound(string playerid, int yearid, string round)
        {
            var items = Context.Pitchingposts
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.round == round);

 
            OnGetPitchingpostByPlayerIdAndYearIdAndRound(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPitchingpostGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPitchingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);
        partial void OnAfterPitchingpostCreated(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> CreatePitchingpost(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost pitchingpost)
        {
            OnPitchingpostCreated(pitchingpost);

            var existingItem = Context.Pitchingposts
                              .Where(i => i.playerID == pitchingpost.playerID && i.yearID == pitchingpost.yearID && i.round == pitchingpost.round)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Pitchingposts.Add(pitchingpost);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(pitchingpost).State = EntityState.Detached;
                throw;
            }

            OnAfterPitchingpostCreated(pitchingpost);

            return pitchingpost;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> CancelPitchingpostChanges(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPitchingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);
        partial void OnAfterPitchingpostUpdated(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> UpdatePitchingpost(string playerid, int yearid, string round, TestingRadzenBlazorApp.Models.Baseball.Pitchingpost pitchingpost)
        {
            OnPitchingpostUpdated(pitchingpost);

            var itemToUpdate = Context.Pitchingposts
                              .Where(i => i.playerID == pitchingpost.playerID && i.yearID == pitchingpost.yearID && i.round == pitchingpost.round)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(pitchingpost);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPitchingpostUpdated(pitchingpost);

            return pitchingpost;
        }

        partial void OnPitchingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);
        partial void OnAfterPitchingpostDeleted(TestingRadzenBlazorApp.Models.Baseball.Pitchingpost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> DeletePitchingpost(string playerid, int yearid, string round)
        {
            var itemToDelete = Context.Pitchingposts
                              .Where(i => i.playerID == playerid && i.yearID == yearid && i.round == round)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPitchingpostDeleted(itemToDelete);


            Context.Pitchingposts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPitchingpostDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPlayersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPlayersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPlayersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Player> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Player>> GetPlayers(Query query = null)
        {
            var items = Context.Players.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPlayersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlayerGet(TestingRadzenBlazorApp.Models.Baseball.Player item);
        partial void OnGetPlayerByLahmanId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Player> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Player> GetPlayerByLahmanId(int lahmanid)
        {
            var items = Context.Players
                              .AsNoTracking()
                              .Where(i => i.lahmanID == lahmanid);

 
            OnGetPlayerByLahmanId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPlayerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPlayerCreated(TestingRadzenBlazorApp.Models.Baseball.Player item);
        partial void OnAfterPlayerCreated(TestingRadzenBlazorApp.Models.Baseball.Player item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Player> CreatePlayer(TestingRadzenBlazorApp.Models.Baseball.Player player)
        {
            OnPlayerCreated(player);

            var existingItem = Context.Players
                              .Where(i => i.lahmanID == player.lahmanID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Players.Add(player);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(player).State = EntityState.Detached;
                throw;
            }

            OnAfterPlayerCreated(player);

            return player;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Player> CancelPlayerChanges(TestingRadzenBlazorApp.Models.Baseball.Player item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPlayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Player item);
        partial void OnAfterPlayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Player item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Player> UpdatePlayer(int lahmanid, TestingRadzenBlazorApp.Models.Baseball.Player player)
        {
            OnPlayerUpdated(player);

            var itemToUpdate = Context.Players
                              .Where(i => i.lahmanID == player.lahmanID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(player);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPlayerUpdated(player);

            return player;
        }

        partial void OnPlayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Player item);
        partial void OnAfterPlayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Player item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Player> DeletePlayer(int lahmanid)
        {
            var itemToDelete = Context.Players
                              .Where(i => i.lahmanID == lahmanid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPlayerDeleted(itemToDelete);


            Context.Players.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPlayerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSalariesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/salaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSalariesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/salaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSalariesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Salary> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Salary>> GetSalaries(Query query = null)
        {
            var items = Context.Salaries.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSalariesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSalaryGet(TestingRadzenBlazorApp.Models.Baseball.Salary item);
        partial void OnGetSalaryByYearIdAndTeamIdAndLgIdAndPlayerId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Salary> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Salary> GetSalaryByYearIdAndTeamIdAndLgIdAndPlayerId(int yearid, string teamid, string lgid, string playerid)
        {
            var items = Context.Salaries
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.lgID == lgid && i.playerID == playerid);

 
            OnGetSalaryByYearIdAndTeamIdAndLgIdAndPlayerId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSalaryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSalaryCreated(TestingRadzenBlazorApp.Models.Baseball.Salary item);
        partial void OnAfterSalaryCreated(TestingRadzenBlazorApp.Models.Baseball.Salary item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Salary> CreateSalary(TestingRadzenBlazorApp.Models.Baseball.Salary salary)
        {
            OnSalaryCreated(salary);

            var existingItem = Context.Salaries
                              .Where(i => i.yearID == salary.yearID && i.teamID == salary.teamID && i.lgID == salary.lgID && i.playerID == salary.playerID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Salaries.Add(salary);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(salary).State = EntityState.Detached;
                throw;
            }

            OnAfterSalaryCreated(salary);

            return salary;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Salary> CancelSalaryChanges(TestingRadzenBlazorApp.Models.Baseball.Salary item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSalaryUpdated(TestingRadzenBlazorApp.Models.Baseball.Salary item);
        partial void OnAfterSalaryUpdated(TestingRadzenBlazorApp.Models.Baseball.Salary item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Salary> UpdateSalary(int yearid, string teamid, string lgid, string playerid, TestingRadzenBlazorApp.Models.Baseball.Salary salary)
        {
            OnSalaryUpdated(salary);

            var itemToUpdate = Context.Salaries
                              .Where(i => i.yearID == salary.yearID && i.teamID == salary.teamID && i.lgID == salary.lgID && i.playerID == salary.playerID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(salary);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSalaryUpdated(salary);

            return salary;
        }

        partial void OnSalaryDeleted(TestingRadzenBlazorApp.Models.Baseball.Salary item);
        partial void OnAfterSalaryDeleted(TestingRadzenBlazorApp.Models.Baseball.Salary item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Salary> DeleteSalary(int yearid, string teamid, string lgid, string playerid)
        {
            var itemToDelete = Context.Salaries
                              .Where(i => i.yearID == yearid && i.teamID == teamid && i.lgID == lgid && i.playerID == playerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSalaryDeleted(itemToDelete);


            Context.Salaries.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSalaryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchoolsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/schools/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchoolsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/schools/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchoolsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.School> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.School>> GetSchools(Query query = null)
        {
            var items = Context.Schools.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchoolsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSchoolGet(TestingRadzenBlazorApp.Models.Baseball.School item);
        partial void OnGetSchoolBySchoolId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.School> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.School> GetSchoolBySchoolId(string schoolid)
        {
            var items = Context.Schools
                              .AsNoTracking()
                              .Where(i => i.schoolID == schoolid);

 
            OnGetSchoolBySchoolId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSchoolGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSchoolCreated(TestingRadzenBlazorApp.Models.Baseball.School item);
        partial void OnAfterSchoolCreated(TestingRadzenBlazorApp.Models.Baseball.School item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.School> CreateSchool(TestingRadzenBlazorApp.Models.Baseball.School school)
        {
            OnSchoolCreated(school);

            var existingItem = Context.Schools
                              .Where(i => i.schoolID == school.schoolID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Schools.Add(school);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(school).State = EntityState.Detached;
                throw;
            }

            OnAfterSchoolCreated(school);

            return school;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.School> CancelSchoolChanges(TestingRadzenBlazorApp.Models.Baseball.School item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSchoolUpdated(TestingRadzenBlazorApp.Models.Baseball.School item);
        partial void OnAfterSchoolUpdated(TestingRadzenBlazorApp.Models.Baseball.School item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.School> UpdateSchool(string schoolid, TestingRadzenBlazorApp.Models.Baseball.School school)
        {
            OnSchoolUpdated(school);

            var itemToUpdate = Context.Schools
                              .Where(i => i.schoolID == school.schoolID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(school);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSchoolUpdated(school);

            return school;
        }

        partial void OnSchoolDeleted(TestingRadzenBlazorApp.Models.Baseball.School item);
        partial void OnAfterSchoolDeleted(TestingRadzenBlazorApp.Models.Baseball.School item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.School> DeleteSchool(string schoolid)
        {
            var itemToDelete = Context.Schools
                              .Where(i => i.schoolID == schoolid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSchoolDeleted(itemToDelete);


            Context.Schools.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSchoolDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchoolsplayersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/schoolsplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/schoolsplayers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchoolsplayersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/schoolsplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/schoolsplayers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchoolsplayersRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer>> GetSchoolsplayers(Query query = null)
        {
            var items = Context.Schoolsplayers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchoolsplayersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSchoolsplayerGet(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);
        partial void OnGetSchoolsplayerByPlayerIdAndSchoolId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> GetSchoolsplayerByPlayerIdAndSchoolId(string playerid, string schoolid)
        {
            var items = Context.Schoolsplayers
                              .AsNoTracking()
                              .Where(i => i.playerID == playerid && i.schoolID == schoolid);

 
            OnGetSchoolsplayerByPlayerIdAndSchoolId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSchoolsplayerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSchoolsplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);
        partial void OnAfterSchoolsplayerCreated(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> CreateSchoolsplayer(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer schoolsplayer)
        {
            OnSchoolsplayerCreated(schoolsplayer);

            var existingItem = Context.Schoolsplayers
                              .Where(i => i.playerID == schoolsplayer.playerID && i.schoolID == schoolsplayer.schoolID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Schoolsplayers.Add(schoolsplayer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(schoolsplayer).State = EntityState.Detached;
                throw;
            }

            OnAfterSchoolsplayerCreated(schoolsplayer);

            return schoolsplayer;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> CancelSchoolsplayerChanges(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSchoolsplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);
        partial void OnAfterSchoolsplayerUpdated(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> UpdateSchoolsplayer(string playerid, string schoolid, TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer schoolsplayer)
        {
            OnSchoolsplayerUpdated(schoolsplayer);

            var itemToUpdate = Context.Schoolsplayers
                              .Where(i => i.playerID == schoolsplayer.playerID && i.schoolID == schoolsplayer.schoolID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(schoolsplayer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSchoolsplayerUpdated(schoolsplayer);

            return schoolsplayer;
        }

        partial void OnSchoolsplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);
        partial void OnAfterSchoolsplayerDeleted(TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> DeleteSchoolsplayer(string playerid, string schoolid)
        {
            var itemToDelete = Context.Schoolsplayers
                              .Where(i => i.playerID == playerid && i.schoolID == schoolid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSchoolsplayerDeleted(itemToDelete);


            Context.Schoolsplayers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSchoolsplayerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSeriespostsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/seriesposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/seriesposts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSeriespostsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/seriesposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/seriesposts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSeriespostsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Seriespost> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Seriespost>> GetSeriesposts(Query query = null)
        {
            var items = Context.Seriesposts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSeriespostsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSeriespostGet(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);
        partial void OnGetSeriespostByYearIdAndRound(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Seriespost> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Seriespost> GetSeriespostByYearIdAndRound(int yearid, string round)
        {
            var items = Context.Seriesposts
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.round == round);

 
            OnGetSeriespostByYearIdAndRound(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSeriespostGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSeriespostCreated(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);
        partial void OnAfterSeriespostCreated(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Seriespost> CreateSeriespost(TestingRadzenBlazorApp.Models.Baseball.Seriespost seriespost)
        {
            OnSeriespostCreated(seriespost);

            var existingItem = Context.Seriesposts
                              .Where(i => i.yearID == seriespost.yearID && i.round == seriespost.round)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Seriesposts.Add(seriespost);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(seriespost).State = EntityState.Detached;
                throw;
            }

            OnAfterSeriespostCreated(seriespost);

            return seriespost;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Seriespost> CancelSeriespostChanges(TestingRadzenBlazorApp.Models.Baseball.Seriespost item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSeriespostUpdated(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);
        partial void OnAfterSeriespostUpdated(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Seriespost> UpdateSeriespost(int yearid, string round, TestingRadzenBlazorApp.Models.Baseball.Seriespost seriespost)
        {
            OnSeriespostUpdated(seriespost);

            var itemToUpdate = Context.Seriesposts
                              .Where(i => i.yearID == seriespost.yearID && i.round == seriespost.round)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(seriespost);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSeriespostUpdated(seriespost);

            return seriespost;
        }

        partial void OnSeriespostDeleted(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);
        partial void OnAfterSeriespostDeleted(TestingRadzenBlazorApp.Models.Baseball.Seriespost item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Seriespost> DeleteSeriespost(int yearid, string round)
        {
            var itemToDelete = Context.Seriesposts
                              .Where(i => i.yearID == yearid && i.round == round)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSeriespostDeleted(itemToDelete);


            Context.Seriesposts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSeriespostDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTeamsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teams/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teams/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTeamsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teams/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teams/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTeamsRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Team> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Team>> GetTeams(Query query = null)
        {
            var items = Context.Teams.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTeamsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTeamGet(TestingRadzenBlazorApp.Models.Baseball.Team item);
        partial void OnGetTeamByYearIdAndLgIdAndTeamId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Team> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Team> GetTeamByYearIdAndLgIdAndTeamId(int yearid, string lgid, string teamid)
        {
            var items = Context.Teams
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.lgID == lgid && i.teamID == teamid);

 
            OnGetTeamByYearIdAndLgIdAndTeamId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTeamGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTeamCreated(TestingRadzenBlazorApp.Models.Baseball.Team item);
        partial void OnAfterTeamCreated(TestingRadzenBlazorApp.Models.Baseball.Team item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Team> CreateTeam(TestingRadzenBlazorApp.Models.Baseball.Team team)
        {
            OnTeamCreated(team);

            var existingItem = Context.Teams
                              .Where(i => i.yearID == team.yearID && i.lgID == team.lgID && i.teamID == team.teamID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Teams.Add(team);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(team).State = EntityState.Detached;
                throw;
            }

            OnAfterTeamCreated(team);

            return team;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Team> CancelTeamChanges(TestingRadzenBlazorApp.Models.Baseball.Team item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTeamUpdated(TestingRadzenBlazorApp.Models.Baseball.Team item);
        partial void OnAfterTeamUpdated(TestingRadzenBlazorApp.Models.Baseball.Team item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Team> UpdateTeam(int yearid, string lgid, string teamid, TestingRadzenBlazorApp.Models.Baseball.Team team)
        {
            OnTeamUpdated(team);

            var itemToUpdate = Context.Teams
                              .Where(i => i.yearID == team.yearID && i.lgID == team.lgID && i.teamID == team.teamID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(team);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTeamUpdated(team);

            return team;
        }

        partial void OnTeamDeleted(TestingRadzenBlazorApp.Models.Baseball.Team item);
        partial void OnAfterTeamDeleted(TestingRadzenBlazorApp.Models.Baseball.Team item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Team> DeleteTeam(int yearid, string lgid, string teamid)
        {
            var itemToDelete = Context.Teams
                              .Where(i => i.yearID == yearid && i.lgID == lgid && i.teamID == teamid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTeamDeleted(itemToDelete);


            Context.Teams.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTeamDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTeamsfranchisesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teamsfranchises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teamsfranchises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTeamsfranchisesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teamsfranchises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teamsfranchises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTeamsfranchisesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise>> GetTeamsfranchises(Query query = null)
        {
            var items = Context.Teamsfranchises.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTeamsfranchisesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTeamsfranchiseGet(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);
        partial void OnGetTeamsfranchiseByFranchId(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> GetTeamsfranchiseByFranchId(string franchid)
        {
            var items = Context.Teamsfranchises
                              .AsNoTracking()
                              .Where(i => i.franchID == franchid);

 
            OnGetTeamsfranchiseByFranchId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTeamsfranchiseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTeamsfranchiseCreated(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);
        partial void OnAfterTeamsfranchiseCreated(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> CreateTeamsfranchise(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise teamsfranchise)
        {
            OnTeamsfranchiseCreated(teamsfranchise);

            var existingItem = Context.Teamsfranchises
                              .Where(i => i.franchID == teamsfranchise.franchID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Teamsfranchises.Add(teamsfranchise);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(teamsfranchise).State = EntityState.Detached;
                throw;
            }

            OnAfterTeamsfranchiseCreated(teamsfranchise);

            return teamsfranchise;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> CancelTeamsfranchiseChanges(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTeamsfranchiseUpdated(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);
        partial void OnAfterTeamsfranchiseUpdated(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> UpdateTeamsfranchise(string franchid, TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise teamsfranchise)
        {
            OnTeamsfranchiseUpdated(teamsfranchise);

            var itemToUpdate = Context.Teamsfranchises
                              .Where(i => i.franchID == teamsfranchise.franchID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(teamsfranchise);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTeamsfranchiseUpdated(teamsfranchise);

            return teamsfranchise;
        }

        partial void OnTeamsfranchiseDeleted(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);
        partial void OnAfterTeamsfranchiseDeleted(TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> DeleteTeamsfranchise(string franchid)
        {
            var itemToDelete = Context.Teamsfranchises
                              .Where(i => i.franchID == franchid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTeamsfranchiseDeleted(itemToDelete);


            Context.Teamsfranchises.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTeamsfranchiseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTeamshalvesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teamshalves/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teamshalves/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTeamshalvesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/baseball/teamshalves/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/baseball/teamshalves/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTeamshalvesRead(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> items);

        public async Task<IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamshalf>> GetTeamshalves(Query query = null)
        {
            var items = Context.Teamshalves.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTeamshalvesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTeamshalfGet(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);
        partial void OnGetTeamshalfByYearIdAndLgIdAndTeamIdAndHalf(ref IQueryable<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> items);


        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> GetTeamshalfByYearIdAndLgIdAndTeamIdAndHalf(int yearid, string lgid, string teamid, string half)
        {
            var items = Context.Teamshalves
                              .AsNoTracking()
                              .Where(i => i.yearID == yearid && i.lgID == lgid && i.teamID == teamid && i.Half == half);

 
            OnGetTeamshalfByYearIdAndLgIdAndTeamIdAndHalf(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTeamshalfGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTeamshalfCreated(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);
        partial void OnAfterTeamshalfCreated(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> CreateTeamshalf(TestingRadzenBlazorApp.Models.Baseball.Teamshalf teamshalf)
        {
            OnTeamshalfCreated(teamshalf);

            var existingItem = Context.Teamshalves
                              .Where(i => i.yearID == teamshalf.yearID && i.lgID == teamshalf.lgID && i.teamID == teamshalf.teamID && i.Half == teamshalf.Half)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Teamshalves.Add(teamshalf);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(teamshalf).State = EntityState.Detached;
                throw;
            }

            OnAfterTeamshalfCreated(teamshalf);

            return teamshalf;
        }

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> CancelTeamshalfChanges(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTeamshalfUpdated(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);
        partial void OnAfterTeamshalfUpdated(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> UpdateTeamshalf(int yearid, string lgid, string teamid, string half, TestingRadzenBlazorApp.Models.Baseball.Teamshalf teamshalf)
        {
            OnTeamshalfUpdated(teamshalf);

            var itemToUpdate = Context.Teamshalves
                              .Where(i => i.yearID == teamshalf.yearID && i.lgID == teamshalf.lgID && i.teamID == teamshalf.teamID && i.Half == teamshalf.Half)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(teamshalf);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTeamshalfUpdated(teamshalf);

            return teamshalf;
        }

        partial void OnTeamshalfDeleted(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);
        partial void OnAfterTeamshalfDeleted(TestingRadzenBlazorApp.Models.Baseball.Teamshalf item);

        public async Task<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> DeleteTeamshalf(int yearid, string lgid, string teamid, string half)
        {
            var itemToDelete = Context.Teamshalves
                              .Where(i => i.yearID == yearid && i.lgID == lgid && i.teamID == teamid && i.Half == half)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTeamshalfDeleted(itemToDelete);


            Context.Teamshalves.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTeamshalfDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}