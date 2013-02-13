require 'factory_girl'

FactoryGirl.define  do
	factory :account_process do |f|
		f.sequence(:name){|n|  "process name#{n}"}
		lastrun Date.current
		f.association :account
	end
end