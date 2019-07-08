using Armoniasoft.ClientDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Armoniasoft.ClientDB
{
    public interface IClientDBDocumentClient
    {
        /// <summary>
        /// Finds a single document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="documentId">Document ID</param>
        /// <returns>Throws an exception if not found</returns>
        Task<T> GetDocument<T>(string tenantId, string documentId, [CallerMemberName] string caller = "");

        /// <summary>
        /// Finds a single document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="documentId">Document ID</param>
        /// <returns>Returns default (null) if not found</returns>
        Task<T> GetDocumentOrDefault<T>(string tenantId, string documentId, [CallerMemberName] string caller = "");

        Task<List<T>> GetDocuments<T>(string tenantId, Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string caller = "");

        /// <summary>
        /// GetDocuments query which returns a subset of parent collection data
        /// </summary>
        /// <typeparam name="T">Collection data type</typeparam>
        /// <typeparam name="R">Output (subset) data type</typeparam>
        /// <param name="tenantId"></param>
        /// <param name="appender">Appender</param>
        /// <returns></returns>
        Task<List<R>> GetDocuments<T, R>(string tenantId, Func<IQueryable<T>, IQueryable<R>> appender, [CallerMemberName] string caller = "");

        Task<List<T>> GetDocumentsCrossPartition<T>(Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string caller = "");

        /// <summary>
        /// GetDocuments cross partition (multi-tenant) query which returns a subset of parent collection data
        /// </summary>
        /// <typeparam name="T">Collection data type</typeparam>
        /// <typeparam name="R">Output (subset) data type</typeparam>
        /// <param name="tenantId"></param>
        /// <param name="appender">Appender</param>
        /// <returns></returns>
        Task<List<R>> GetDocumentsCrossPartition<T,R>(Func<IQueryable<T>, IQueryable<R>> appender, [CallerMemberName] string caller = "");

        Task<int> CountDocuments<T>(string tenantId, Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string caller = "");

        Task<T> CreateDocument<T>(string tenantId, T document, AuditUser auditUser = null, [CallerMemberName] string caller = "");

        Task<T> UpdateDocument<T>(string tenantId, string documentId, T document, string ETag, AuditUser auditUser = null, [CallerMemberName] string caller = "");
    }
}