require 'spec_helper'

describe ComputersController do

  describe "Routing" do

		#view
		it "GET does route to #index" do
			{get: "computers"}.should route_to(
				controller: "computers", 
				action: "index")
		end

		it "POST routes to #create" do
			{post: "computers/create"}.should route_to(
				controller: "computers", 
				action: "create")
		end

		#view
		it "GET routes to #new" do
			{get: "computers/new"}.should route_to(
				controller: "computers", 
				action: "new")
		end

		# view
		it "GET routes to #edit" do
			{get: "computers/edit/1"}.should route_to(
				controller: "computers", 
				id: "1",
				action: "edit")
		end

		#view
		it "GET does route to #show" do
			{get: "computers/show/1"}.should route_to(
				controller: "computers", 
				action: "show", 
				id: "1")
		end

		it "PUT does route to #update" do
			{put: "computers/update/1"}.should route_to(
				controller: "computers", 
				action: "update", 
				id: "1") 
		end

		it "DELETE routes to #destroy" do
			{delete: "computers/destroy/1"}.should route_to(
				controller: "computers", 
				action: "destroy",
				id: "1")
		end
	end

end
