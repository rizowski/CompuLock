require "spec_helper"

describe Api::V1::AccountsController do
	describe "Routing" do

		it "GET does route to #index" do
			{get: "api/v1/accounts"}.should route_to(
				controller: "api/v1/accounts", 
				action: "index", 
				format: "json")
		end

		it "GET does route to #show" do
			{get: "api/v1/accounts/1"}.should route_to(
				controller: "api/v1/accounts", 
				action: "show", 
				id: "1",
				format: "json")
		end

		it "POST routes to #create" do
			{post: "api/v1/accounts"}.should route_to(
				controller: "api/v1/accounts", 
				action: "create", 
				format: "json")
		end

		it "PUT does route to #update" do
			{put: "api/v1/accounts/1"}.should route_to(
				controller: "api/v1/accounts", 
				action: "update", 
				id: "1",
				format: "json") 
		end

		it "DELETE routes to #destroy" do
			{delete: "api/v1/accounts/1"}.should route_to(
				controller: "api/v1/accounts", 
				action: "destroy",
				id: "1",
				format: "json")
		end
	end
end