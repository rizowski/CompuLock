
# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

first_user = User.create(:email => "crouska@gmail.com", :password=> "190421", :password_confirmation => "190421")
	first_comp = Computer.create(:user_id=> first_user.id, :name => "Moms Desktop", :enviroment => "Windows XP")
		Account.create(:computer_id => first_comp.id, :domain => "WORKGROUP", :username => "Parents", :tracking => false)
		first_account = Account.create(:computer_id => first_comp.id, :domain => "WORKGROUP", :username => "Kids", :tracking => true)
			AccountHistory.create(:account_id => first_account.id, :title=> "Facebook", :domain => "facebook.com", :url => "/rizowski")
			AccountHistory.create(:account_id => first_account.id, :title=> "Google.com", :domain => "google.com")
			AccountHistory.create(:account_id => first_account.id, :title=> "Wallbase", :domain => "wallbase.cc")
			AccountHistory.create(:account_id => first_account.id, :title=> "Rizowski", :domain => "twitter.com", :url => "/rizowski")
			AccountProcess.create(:account_id => first_account.id, :name => "System32")
			AccountProcess.create(:account_id => first_account.id, :name => "chrome")
			AccountProcess.create(:account_id => first_account.id, :name => "firefox")
			AccountProcess.create(:account_id => first_account.id, :name => "explorer")
			AccountProgram.create(:account_id => first_account.id, :name => "Chrome", :open_count => 105)
			AccountProgram.create(:account_id => first_account.id, :name => "iexplore", :open_count => 1)
	second_comp = Computer.create(:user_id=> first_user.id, :name => "Gavins Desktop", :enviroment => "Windows XP")
		Account.create(:computer_id => second_comp.id, :domain => "WORKGROUP", :username => "Gavin", :tracking => false)
	third_comp = Computer.create(:user_id=> first_user.id, :name => "Gavins Laptop", :enviroment => "Windows XP")
		Account.create(:computer_id => third_comp.id, :domain => "WORKGROUP", :username => "Gavin", :tracking => false)

second_user = User.create(:email => "cbacon@gmail.com", :password => "190421", :password_confirmation => "190421")
	fourth_comp = Computer.create(:user_id=>second_user.id, :name => "My Laptop", :enviroment => "Windows Seven")
		Account.create(:computer_id => fourth_comp.id, :domain => "NUSTUDENT", :username => "Bacon", :tracking => false)


