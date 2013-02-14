require 'factory_girl'

FactoryGirl.define  do
	factory :account_history do |f|
		account
		f.sequence(:domain){|n| "WorkGroup#{n}"}
		last_visited 10.days.ago
	end
end