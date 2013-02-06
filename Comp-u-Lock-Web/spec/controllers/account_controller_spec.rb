require 'spec_helper'

describe AccountController do
	describe "Routing" do

		#view
		it "GET does route to #index" do
			{get: "account"}.should route_to(
				controller: "account", 
				action: "index")
		end

		it "POST routes to #create" do
			{post: "account"}.should route_to(
				controller: "account", 
				action: "create")
		end

		#view
		it "GET routes to #new" do
			{get: "account/new"}.should route_to(
				controller: "account", 
				action: "new")
		end

		# view
		it "GET routes to #edit" do
			{get: "account/1/edit"}.should route_to(
				controller: "account", 
				action: "edit",
				id: "1")
		end

		#view
		it "GET does route to #show" do
			{get: "account/1"}.should route_to(
				controller: "account", 
				action: "show", 
				id: "1")
		end

		it "PUT does route to #update" do
			{put: "account/1"}.should route_to(
				controller: "account", 
				action: "update", 
				id: "1") 
		end

		it "DELETE routes to #destroy" do
			{delete: "account/1"}.should route_to(
				controller: "account", 
				action: "destroy",
				id: "1")
		end
	end
end
