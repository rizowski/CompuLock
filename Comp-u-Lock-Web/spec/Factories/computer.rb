require 'factory_girl'

FactoryGirl.define  do
	factory :computer do
		name "Desktop"
		ip_address "192.168.1.12"
		enviroment "Windows XP"
	end
end