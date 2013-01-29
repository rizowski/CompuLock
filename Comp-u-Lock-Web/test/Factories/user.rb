require 'factory_girl'

FactoryGirl.define  do
	factory :user do
		email "testing@gmail.com"
		encrypted_password "gfdgfdgdgreygfsawt"
	end
end