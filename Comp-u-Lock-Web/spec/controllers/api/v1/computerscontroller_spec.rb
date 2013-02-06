require "spec_helper"

describe Api::V1::ComputersController do
	describe "Routing" do
		it "GET does route to #index" do
			{get: "api/v1/computers"}.should route_to(
				controller: "api/v1/computers", 
				action: "index", 
				format: "json")
		end

		it "GET does route to #show" do
			{get: "api/v1/computers/1"}.should route_to(
				controller: "api/v1/computers", 
				action: "show", 
				id: "1",
				format: "json")
		end

		it "POST routes to #create" do
			{post: "api/v1/computers"}.should route_to(
				controller: "api/v1/computers", 
				action: "create", 
				format: "json")
		end

		it "PUT does route to #update" do
			{put: "api/v1/computers/1"}.should route_to(
				controller: "api/v1/computers", 
				action: "update", 
				id: "1",
				format: "json") 
		end

		it "DELETE routes to #destroy" do
			{delete: "api/v1/computers/1"}.should route_to(
				controller: "api/v1/computers", 
				action: "destroy",
				id: "1",
				format: "json")
		end
	end
end