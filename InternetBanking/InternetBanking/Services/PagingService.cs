using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Services
{
    public enum PagingMode
    {
        Paging,
        NoPaging
    }

    public interface IHomePagingService
    {
        Task<PagedObject<T>> DoPage<T>(IQueryable<T> items, int page) where T : IPageble;
    }

    public class PagingService : IHomePagingService
    {
        public virtual async Task<PagedObject<T>> DoPage<T>(IQueryable<T> items, int page) where T : IPageble
        {
            int pageSize = 10;
            var count = await items.CountAsync();
            var objects = await items.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedObject<T>(count, pageSize, objects);
        }
    }

    public class NoPagingService : PagingService, IHomePagingService
    {
        public async Task<PagedObject<T>>  DoPage<T>(IQueryable<T> items, int page) where T : IPageble
        {
            var count = await items.CountAsync();
            int pageSize = count;
            return new PagedObject<T>(count, pageSize, items.ToList());
        }
    }

    public class PagedObject<T> where T : IPageble
    {
        private int count;
        private int pageSize;
        private List<T> objects;

        public int Count => count;

        public int PageSize => pageSize;

        public List<T> Objects => objects;

        public PagedObject(int count, int pageSize, List<T> objects)
        {
            this.count = count;
            this.objects = objects;
            this.pageSize = pageSize;
        }
    }
}
