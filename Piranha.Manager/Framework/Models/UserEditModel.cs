﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

using AutoMapper;
using Piranha.Entities;
using Piranha.Web;

namespace Piranha.Manager.Models
{
	/// <summary>
	/// Edit model for the user eidt view.
	/// </summary>
	public class UserEditModel
	{
		#region Properties
		/// <summary>
		/// Gets/sets the current user.
		/// </summary>
		public Entities.User User { get ; set ; }

		/// <summary>
		/// Gets/sets the available groups.
		/// </summary>
		public IList<SelectListItem> Groups { get ; set ; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public UserEditModel() {
			using (var db = new DataContext()) {
				Groups = Mapper.Map<List<SelectListItem>>(db.Groups.OrderBy(g => g.Name).ToList()) ;
				User = new Entities.User() ;
			}
		}

		/// <summary>
		/// Gets the edit model for the given id.
		/// </summary>
		/// <param name="id">The id</param>
		/// <returns>The model</returns>
		public static UserEditModel GetById(Guid id) {
			var m = new UserEditModel() ;

			using (var db = new DataContext()) {
				m.User = db.Users.Where(u => u.Id == id).Single() ;
			}
			return m ;
		}

		/// <summary>
		/// Saves the current edit model.
		/// </summary>
		/// <returns>If the entity was updated in the database.</returns>
		public bool Save() {
			using (var db = new DataContext()) {
				var user = db.Users.Where(u => u.Id == User.Id).SingleOrDefault() ;
				if (user == null) {
					user = new Entities.User() ;
					user.Attach(db, EntityState.Added) ;
				}
				Mapper.Map<Entities.User, Entities.User>(User, user) ;

				if (!String.IsNullOrEmpty(User.Password))
					user.Password = Piranha.Models.SysUser.Encrypt(User.Password) ;

				var ret = db.SaveChanges() > 0 ;
				User.Id = user.Id ;

				return ret ;
			}
		}
	}
}