require "spec_helper"
describe Api::V1::UsersController do
	describe "Routing" do

		it "GET does route to #index" do
			{get: "api/v1/users"}.should route_to(
				controller: "api/v1/users", 
				action: "index", 
				format: "json")
		end

		it "GET does NOT route to #show" do
			{get: "api/v1/users"}.should_not route_to(
				controller: "api/v1/users", 
				action: "show", 
				format: "json")
		end

		it "PUT does route to #update" do
			{put: "api/v1/users/1"}.should route_to(
				controller: "api/v1/users", 
				action: "update", 
				id: "1",
				format: "json") 
		end
	end
end