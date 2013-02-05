require "spec_helper"

describe Api::V1::TokensController do
	describe "routing" do
		it "routes to #create" do
			get("api/v1/tokens").should route_to("tokens#create")
		end

		it "routes to #destroy" do
			get("api/v1/tokens").should route_to("tokens#destroy")
		end
	end
end