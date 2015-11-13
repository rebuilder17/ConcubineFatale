function test1()
	test2()
end

function test2()
	wrongfunction.call() -- ERROR
	print "bbbb"
end

print "aaaa"
test1()
