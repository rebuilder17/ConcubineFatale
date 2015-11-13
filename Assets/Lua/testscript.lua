return function()
	print "BOO!"
	csharpfunc()
	coroutine.yield()
	print "BOO! 2"
end