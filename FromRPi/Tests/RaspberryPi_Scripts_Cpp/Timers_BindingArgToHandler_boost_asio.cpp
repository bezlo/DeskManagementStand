#include <iostream>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>

void print(const boost::system::error_code& /*e*/,
		   boost::asio::deadline_timer* t, int* count)
{
	if (*count < 5)
	{
		std::cout << *count << "\n" << std::endl;
		++(*count);
		// expires_at() show actual time, as argument set new time
		//(*t).expires_at((*t).expires_at() + boost::posix_time::seconds(1));
		t->expires_at(t->expires_at() + boost::posix_time::seconds(1));
		t->async_wait(boost::bind(
			print, boost::asio::placeholders::error, t, count));
	}
}

int main()
{
	boost::asio::io_service io;
	int count = 0 ;
	boost::asio::deadline_timer t(io, boost::posix_time::seconds(1));
	
	t.async_wait(boost::bind(print,
	boost::asio::placeholders::error, &t, &count));
	
	io.run();
	
	std::cout << "Final count is:" << count << "\n";
	
	return 0;
}
