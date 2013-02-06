require 'spec_helper'

# describe "Home page" do 
# 	it "displays successful login after logging in successfully" do
# 		user = User.create!(:email=>"test@test.com", :password=>"sfsdggert")
# 		visit :new_user_session
# 		fill_in "Username", :with => "test@test.com"
# 		fill_in "Password", :with => "sfsdggert"
# 		click_button "Log in"

# 		expect(page).to have_content 'successfully'
# 	end

# 	it "displays logged out after logging out successfully" do 
# 		user = User.create!(:email=>"test@test.com", :password=>"sfsdggert")
# 		visit :new_user_session
# 		fill_in "Username", :with => "test@test.com"
# 		fill_in "Password", :with => "sfsdggert"
# 		click_button "Log in"
# 		click_link "logout" 
# 		expect(page).to have_content 'successfully'
# 	end
# end