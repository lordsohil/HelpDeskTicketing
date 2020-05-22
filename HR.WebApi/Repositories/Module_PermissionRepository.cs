﻿using HR.WebApi.DAL;
using HR.WebApi.Exceptions;
using HR.WebApi.Interfaces;
using HR.WebApi.Model;
using HR.WebApi.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace HR.WebApi.Repositories
{
    public class Module_PermissionRepository<T> : ICommonRepository<Module_PermissionView>, ICommonQuery<Module_PermissionView>
    {
        private readonly ApplicationDbContext adbContext;

        public Module_PermissionRepository(ApplicationDbContext applicationDbContext)
        {
            adbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Module_PermissionView>> GetAll(int RecordLimit)
        {
            try
            {
                IEnumerable<Module_PermissionView> vList;
                if (RecordLimit > 0)
                {
                    vList = (from mp in adbContext.module_permission
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                                ).Take(RecordLimit).ToList();
                }
                else
                {
                    vList = (from mp in adbContext.module_permission
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                                ).ToList();
                }
                if (vList == null || vList.Count() == 0)
                    throw new RecoredNotFoundException("Get Data Empty");

                return await Task.FromResult(vList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Module_PermissionView>> Get(int id)
        {
            try
            {
                var vList = (from mp in adbContext.module_permission
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             where mp.Id == id
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                                ).ToList();
                if (vList == null || vList.Count() == 0)
                    throw new RecoredNotFoundException("Get Data Empty");

                return await Task.FromResult(vList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Module_PermissionView>> GetBy(SearchBy searchBy)
        {
            try
            {
                var vList = (from mp in adbContext.module_permission.Where(String.Format("{0}=={1}", searchBy.FieldName, searchBy.FieldValue))
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                            ).Take(searchBy.RecordLimit).ToList();
                if (vList == null || vList.Count() == 0)
                    throw new RecoredNotFoundException("Get Data Empty");

                return await Task.FromResult(vList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Module_PermissionView>> FindPaginated(int pageIndex, int pageSize, string searchValue)
        {
            try
            {
                IEnumerable<Module_PermissionView> vList;
                if (String.IsNullOrEmpty(searchValue))
                {
                    //Find with Paging
                    vList = (from mp in adbContext.module_permission
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                                ).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                }
                else
                {
                    //Find with Paging & Searching
                    vList = (from mp in adbContext.module_permission.Where(w => new[] { w.Name.ToLower() }.Any(a => a.Contains(searchValue.ToLower())))
                             join m in adbContext.module on mp.Module_Id equals m.Id
                             where mp.isActive == 1 && m.isActive == 1
                             select new Module_PermissionView
                             {
                                 Id = mp.Id,
                                 Module_Id = mp.Module_Id,
                                 Module_Name = m.Name,
                                 Name = mp.Name,
                                 isActive = mp.isActive
                             }
                               ).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                }
                if (vList == null || vList.Count() == 0)
                    throw new RecoredNotFoundException("Get Data Empty");

                return await Task.FromResult(vList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Insert(Module_PermissionView entity)
        {
            try
            {
                var vList = new Module_Permission
                {
                    Module_Id = entity.Module_Id,
                    Name = entity.Name,
                    isActive = entity.isActive,
                    AddedBy = entity.AddedBy,
                    AddedOn = DateTime.Now
                };
                adbContext.module_permission.Add(vList);
                await Task.FromResult(adbContext.SaveChanges());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(Module_PermissionView entity)
        {
            try
            {
                var vList = adbContext.module_permission.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (vList == null)
                    throw new RecoredNotFoundException("Data Not Available");

                vList.Id = entity.Id;
                vList.Module_Id = entity.Module_Id;
                vList.Name = entity.Name;
                vList.isActive = entity.isActive;

                adbContext.module_permission.Update(vList);

                await Task.FromResult(adbContext.SaveChanges());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ToogleStatus(int id, short isActive)
        {
            try
            {
                var vList = adbContext.module_permission.Where(w => w.Id == id && w.isActive != isActive).ToList().SingleOrDefault();
                if (vList == null)
                    throw new RecoredNotFoundException("Data Not Available");

                vList.isActive = isActive;
                adbContext.module_permission.Update(vList);
                await Task.FromResult(adbContext.SaveChanges());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var vList = adbContext.module_permission.Where(w => w.Id == id).ToList().SingleOrDefault();
                if (vList == null)
                    throw new RecoredNotFoundException("Data Not Available");
                adbContext.module_permission.Remove(vList);
                await Task.FromResult(adbContext.SaveChanges());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exists(Module_PermissionView entity)
        {
            try
            {
                int intCount = 0;
                if (entity.Id > 0)
                    intCount = adbContext.module_permission.Where(w => w.Id != entity.Id && (w.Name == entity.Name && w.Module_Id == entity.Module_Id)).Count();
                else
                    intCount = adbContext.module_permission.Where(w => w.Id != entity.Id && (w.Name == entity.Name && w.Module_Id == entity.Module_Id)).Count();
                return (intCount > 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RecordCount(string searchValue)
        {
            try
            {
                if (String.IsNullOrEmpty(searchValue))
                {
                    //Find all no of rows
                    var vCount = (from mp in adbContext.module_permission
                                  join m in adbContext.module on mp.Module_Id equals m.Id
                                  select mp.Id).Count();
                    return vCount;
                }
                else
                {
                    //Find no of rows with Searching 
                    var vCount = (from mp in adbContext.module_permission.Where(w => new[] { w.Name.ToLower() }.Any(a => a.Contains(searchValue.ToLower())))
                                  join m in adbContext.module on mp.Module_Id equals m.Id
                                  select mp.Id).Count();
                    return vCount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}