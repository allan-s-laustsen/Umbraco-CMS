﻿using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Repositories;
using Umbraco.Core.Persistence.UnitOfWork;

namespace Umbraco.Web.Services
{
    /// <summary>
    /// Represents the Macro Service, which is an easy access to operations involving <see cref="IMacro"/>
    /// </summary>
    public class MacroService : IMacroService
    {
        private readonly IUnitOfWorkProvider _provider;

        public MacroService()
            : this(new FileUnitOfWorkProvider())
        {
        }

        public MacroService(IUnitOfWorkProvider provider)
        {
            _provider = provider;
            EnsureMacroCache();
        }

        /// <summary>
        /// Ensures the macro cache by getting all macros
        /// from the repository and thus caching them.
        /// </summary>
        private void EnsureMacroCache()
        {
            IEnumerable<IMacro> macros = GetAll();
        }

        /// <summary>
        /// Gets an <see cref="IMacro"/> object by its alias
        /// </summary>
        /// <param name="alias">Alias to retrieve an <see cref="IMacro"/> for</param>
        /// <returns>An <see cref="IMacro"/> object</returns>
        public IMacro GetByAlias(string alias)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMacroRepository, IMacro, string>(unitOfWork);
            return repository.Get(alias);
        }

        /// <summary>
        /// Gets a list all available <see cref="IMacro"/> objects
        /// </summary>
        /// <param name="aliases">Optional array of aliases to limit the results</param>
        /// <returns>An enumerable list of <see cref="IMacro"/> objects</returns>
        public IEnumerable<IMacro> GetAll(params string[] aliases)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMacroRepository, IMacro, string>(unitOfWork);
            return repository.GetAll(aliases);
        }

        /// <summary>
        /// Deletes an <see cref="IMacro"/>
        /// </summary>
        /// <param name="macro"><see cref="IMacro"/> to delete</param>
        public void Delete(IMacro macro)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMacroRepository, IMacro, string>(unitOfWork);
            repository.Delete(macro);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Saves an <see cref="IMacro"/>
        /// </summary>
        /// <param name="macro"><see cref="IMacro"/> to save</param>
        public void Save(IMacro macro)
        {
            var unitOfWork = _provider.GetUnitOfWork();
            var repository = RepositoryResolver.ResolveByType<IMacroRepository, IMacro, string>(unitOfWork);
            repository.AddOrUpdate(macro);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Gets a list all available <see cref="IMacroPropertyType"/> plugins
        /// </summary>
        /// <returns>An enumerable list of <see cref="IMacroPropertyType"/> objects</returns>
        public IEnumerable<IMacroPropertyType> GetMacroPropertyTypes()
        {
            return MacroPropertyTypeResolver.Current.MacroPropertyTypes;
        }

        /// <summary>
        /// Gets an <see cref="IMacroPropertyType"/> by its alias
        /// </summary>
        /// <param name="alias">Alias to retrieve an <see cref="IMacroPropertyType"/> for</param>
        /// <returns>An <see cref="IMacroPropertyType"/> object</returns>
        public IMacroPropertyType GetMacroPropertyTypeByAlias(string alias)
        {
            return MacroPropertyTypeResolver.Current.MacroPropertyTypes.FirstOrDefault(x => x.Alias == alias);
        }
    }
}