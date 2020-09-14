using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Xamarin.Forms;

namespace BiStatApp.Models
{
	public class ShootingBout
	{
		public enum PositionEnum
		{
			PRONE = 1,
			STANDING = 2
		}

		[Key]
		public int Id { get; set; }

		public PositionEnum Position { get; set; }

		public bool Alpha { get; set; }

		public bool Bravo { get; set; }

		public bool Charlie { get; set; }

		public bool Delta { get; set; }

		public bool Echo { get; set; }

		public int SessionId { get; set; }
		public Session Session { get; set; }

		[NotMapped]
		public int Misses
		{
			get 
			{
				int m = 0;

				m = Alpha ? 0 : 1;
				m += Bravo ? 0 : 1;
				m += Charlie ? 0 : 1;
				m += Delta ? 0 : 1;
				m += Echo ? 0 : 1;

				return m;
			}
		}

		//[NotMapped]
		//public ImageSource PositionImage
		//{
		//	get
		//	{
		//		if (Position == PositionEnum.PRONE)
		//		{
		//			return ImageSource.FromResource("BiProne.png");
		//		}
		//		return ImageSource.FromResource("BiStanding.png");
		//	}
		//}
	}
}
