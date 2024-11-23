using System;

namespace XSkills;

[Flags]
public enum EnumNightVisionMode
{
	FilterNone = 0,
	FilterSepia = 1,
	FilterGray = 2,
	FilterGreen = 4,
	FilterBlue = 8,
	FilterRed = 0x10,
	Deactivated = 0x20,
	Filter = 0x3F,
	Compress = 0x40,
	Default = 0x41
}
