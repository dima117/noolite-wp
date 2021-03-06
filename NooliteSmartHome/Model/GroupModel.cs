﻿using NooliteSmartHome.Gateway.Configuration;
using NooliteSmartHome.Helpers;

namespace NooliteSmartHome.Model
{
	public class GroupModel
	{
		public GroupModel(Pr1132ControlGroup group, IconOfGroup icon, int index)
		{
			Index = index;
			Name = group.Name;
			IconPath = icon.GetIconPath();
		}

		public int Index { get; set; }

		public string Name { get; set; }

		public string IconPath { get; set; }
	}
}