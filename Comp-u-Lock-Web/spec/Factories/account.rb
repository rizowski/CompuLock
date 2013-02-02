require 'factory_girl'

FactoryGirl.define  do
	factory :account do
		computer
		domain "WORKGROUP"
		user_name "Rizowski"
		tracking false
		allotted_time 10.seconds
	end
end