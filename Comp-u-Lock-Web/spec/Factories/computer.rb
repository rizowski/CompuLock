require 'factory_girl'

FactoryGirl.define  do
	factory :computer do |f|
		user
		f.sequence(:name){|n| "Desktop#{n}"} 
		ip_address "192.168.1.12"
		enviroment "Windows XP"
	end
end