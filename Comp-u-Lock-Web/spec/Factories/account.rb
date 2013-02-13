require 'factory_girl'

FactoryGirl.define  do
	factory :account do
		computer
		domain "WORKGROUP"
		username "Rizowski"
		tracking false
		allotted_time Time.at(1000)
		used_time Time.at(100)
	end
end