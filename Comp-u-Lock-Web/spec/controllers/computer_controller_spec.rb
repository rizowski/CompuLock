require 'spec_helper'

describe ComputerController do

  describe "Routing" do

		#view
		it "GET does route to #index" do
			{get: "computer"}.should route_to(
				controller: "computer", 
				action: "index")
		end

		it "POST routes to #create" do
			{post: "computer"}.should route_to(
				controller: "computer", 
				action: "create")
		end

		#view
		it "GET routes to #new" do
			{get: "computer/new"}.should route_to(
				controller: "computer", 
				action: "new")
		end

		# view
		it "GET routes to #edit" do
			{get: "computer/edit/1"}.should route_to(
				controller: "computer", 
				id: "1",
				action: "edit")
		end

		#view
		it "GET does route to #show" do
			{get: "computer/1"}.should route_to(
				controller: "computer", 
				action: "show", 
				id: "1")
		end

		it "PUT does route to #update" do
			{put: "computer/1"}.should route_to(
				controller: "computer", 
				action: "update", 
				id: "1") 
		end

		it "DELETE routes to #destroy" do
			{delete: "computer/1"}.should route_to(
				controller: "computer", 
				action: "destroy",
				id: "1")
		end
	end

end
