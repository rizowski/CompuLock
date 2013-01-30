
# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

first_user = User.create(:email => "crouska@gmail.com", :password=> "190421", :password_confirmation => "190421", :admin => true)
	first_comp = Computer.create(:user_id=> first_user.id, :name => "Moms Desktop", :enviroment => "Windows XP")
		Account.create(:computer_id => first_comp.id, :domain => "WORKGROUP", :user_name => "Parents", :tracking => false)
		first_account = Account.create(:computer_id => first_comp.id, :domain => "WORKGROUP", :user_name => "Kids", :tracking => true)
			AccountHistory.create(:account_id => first_account.id, :title=> "Facebook", :domain => "facebook.com", :url => "/rizowski")
			AccountHistory.create(:account_id => first_account.id, :title=> "Google.com", :domain => "google.com")
			AccountHistory.create(:account_id => first_account.id, :title=> "Wallbase", :domain => "wallbase.cc")
			AccountHistory.create(:account_id => first_account.id, :title=> "Rizowski", :domain => "twitter.com", :url => "/rizowski")
			AccountProcess.create(:account_id => first_account.id, :name => "System32", :lastrun => DateTime.now)
			AccountProcess.create(:account_id => first_account.id, :name => "chrome", :lastrun => DateTime.now)
			AccountProcess.create(:account_id => first_account.id, :name => "firefox", :lastrun => DateTime.now)
			AccountProcess.create(:account_id => first_account.id, :name => "explorer", :lastrun => DateTime.now)
			AccountProgram.create(:account_id => first_account.id, :name => "Chrome", :lastrun => DateTime.now, :open_count => 105)
			AccountProgram.create(:account_id => first_account.id, :name => "iexplore", :lastrun => DateTime.now, :open_count => 1)
	second_comp = Computer.create(:user_id=> first_user.id, :name => "Gavins Desktop", :enviroment => "Windows XP")
		Account.create(:computer_id => second_comp.id, :domain => "WORKGROUP", :user_name => "Gavin", :tracking => false)
	third_comp = Computer.create(:user_id=> first_user.id, :name => "Gavins Laptop", :enviroment => "Windows XP")
		Account.create(:computer_id => third_comp.id, :domain => "WORKGROUP", :user_name => "Gavin", :tracking => false)

second_user = User.create(:email => "cbacon@gmail.com", :password => "190421", :password_confirmation => "190421")
	fourth_comp = Computer.create(:user_id=>second_user.id, :name => "My Laptop", :enviroment => "Windows Seven")
		Account.create(:computer_id => fourth_comp.id, :domain => "NUSTUDENT", :user_name => "Bacon", :tracking => false)


