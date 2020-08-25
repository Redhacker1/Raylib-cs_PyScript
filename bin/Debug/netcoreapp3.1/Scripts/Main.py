import clr
clr.AddReference('raylib_cs')
from Raylib_cs import *
import time

Raylib.DrawText("Error!", 0,100, 60, Color.RED)

def Main():
	string_1 = "God"
	String_2 = " Knows"
	string_item = string_1 + String_2

	Raylib.DrawText(string_item, 0,0, 60, Color.GREEN)



