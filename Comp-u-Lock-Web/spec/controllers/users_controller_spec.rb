require 'spec_helper'

describe UsersController do
	describe "Devise Routing" do
		it "POST routes to devise#create" do
			{post: "users"}.should route_to(
				controller: "devise/registrations", 
				action: "create")
		end

		# view
		it "GET routes to devise#edit" do
			{get: "users/edit"}.should route_to(
				controller: "devise/registrations", 
				action: "edit")
		end
	end
  	describe "Routing" do
	    #view
		it "GET does route to #index" do
			{get: "users"}.should route_to(
				controller: "users", 
				action: "index")
		end

		it "POST does NOT route to users#create" do
			{post: "users"}.should_not route_to(
				controller: "users", 
				action: "create")
		end

		it "GET does NOT route to users#edit" do
			{get: "users/edit"}.should_not route_to(
				controller: "users", 
				action: "edit")
		end
  	end
end