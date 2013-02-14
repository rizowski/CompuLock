require 'factory_girl'

FactoryGirl.define  do
	factory :account_program do |f|
		account
		lastrun 5.days.ago
		f.sequence(:name) {|n| "program #{n}"}
	end
end