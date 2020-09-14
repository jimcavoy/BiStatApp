using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xamarin.Forms.Internals;

namespace BiStatApp.ViewModels
{
	public class SessionViewModel : BaseViewModel
	{
		public int Id { get; set; }

		private DateTime _dateTime;
		public DateTime DateTime 
		{
			get { return _dateTime; }
			set 
			{
				SetValue(ref _dateTime, value);
				OnPropertyChanged(nameof(DateTime));
			}
		}

		public SessionViewModel() 
		{
			DateTime = System.DateTime.Now;
		}

		public SessionViewModel(Session session)
		{
			Id = session.Id;
			DateTime = session.DateTime;
			_name = session.Name;
			_description = session.Description;
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				SetValue(ref _name, value);
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set 
			{
				SetValue(ref _description, value);
				OnPropertyChanged(nameof(Description));
			}
		}


	}
}
