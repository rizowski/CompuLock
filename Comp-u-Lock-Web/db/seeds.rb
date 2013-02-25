
# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

first_user = 	User.create(	:email => "crouska@gmail.com", 	:password => "190421", 	:password_confirmation => "190421")
second_user = 	User.create(	:email => "cbacon@gmail.com", 	:password => "190421", 	:password_confirmation => "190421")
	first_comp = 	Computer.create(	:user_id=> first_user.id, 	:name => "Moms Desktop", 	:enviroment => "Windows XP")
	second_comp = 	Computer.create(	:user_id=> first_user.id, 	:name => "Gavins Desktop", 	:enviroment => "Windows XP")
	third_comp = 	Computer.create(	:user_id=> first_user.id, 	:name => "Gavins Laptop", 	:enviroment => "Windows XP")
	fourth_comp = 	Computer.create(	:user_id=> second_user.id, 	:name => "My Laptop", 		:enviroment => "Windows Seven")
						Account.create(:computer_id => first_comp.id, 	:admin => true, 	:domain => "WORKGROUP", 	:username => "Parents", 	:tracking => false)
		first_account = Account.create(:computer_id => first_comp.id, 	:admin => false, 	:domain => "WORKGROUP", 	:username => "Kids", 		:tracking => true)
						Account.create(:computer_id => second_comp.id, 	:admin => true, 	:domain => "WORKGROUP", 	:username => "Gavin", 		:tracking => false)
						Account.create(:computer_id => third_comp.id, 	:admin => true, 	:domain => "WORKGROUP", 	:username => "Gavin", 		:tracking => false)
						Account.create(:computer_id => fourth_comp.id, 	:admin => false,	:domain => "NUSTUDENT", 	:username => "Bacon", 		:tracking => false)


			History.create(:computer_id => first_comp.id,	:title => "Facebook.com", 		:url => "http://facebook.com", 			:visit_count =>  535)
			History.create(:computer_id => first_comp.id, 	:title => "WallBase.cc", 		:url => "http://wallbase.cc", 			:visit_count => 1000)
			History.create(:computer_id => first_comp.id, 	:title => "Twitter(Rizowski)", 	:url => "http://twitter.com/rizowski", 	:visit_count => 1)

			AccountProcess.create(:account_id => first_account.id, 	:name => "System32")
			AccountProcess.create(:account_id => first_account.id, 	:name => "chrome")
			AccountProcess.create(:account_id => first_account.id, 	:name => "firefox")
			AccountProcess.create(:account_id => first_account.id, 	:name => "explorer")
	
		
	
		


